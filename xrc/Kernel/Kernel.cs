using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using xrc.SiteManager;
using xrc.Configuration;
using System.IO;
using System.Reflection;
using xrc.Renderers;
using System.Web;

namespace xrc
{
    public class Kernel : IKernel
    {
        private IMashupParserService _parser;
        private ISiteConfigurationProviderService _siteConfigurationProvider;
        private IMashupLocatorService _fileLocator;
        private string _workingPath;

		#region Static code
        private static Kernel _current;
        public static Kernel Current
		{
			get { return _current; }
		}
		public static void Start(Kernel kernel)
		{
			kernel.Init();
            _current = kernel;
		}
		#endregion

        public Kernel(string workingPath)
        {
            _workingPath = workingPath;
		}

        protected virtual void Init()
        {
            BootstrapContainer();

            _parser = _container.Resolve<IMashupParserService>();
            _siteConfigurationProvider = _container.Resolve<ISiteConfigurationProviderService>();
            _fileLocator = _container.Resolve<IMashupLocatorService>();

            Modules.Add(new Module("Kernel", typeof(IKernel)));
            Modules.Add(new Module("Context", typeof(IContext)));
            Modules.Add(new Module("File", typeof(Modules.IFileModule)));
            Modules.Add(new Module("Html", typeof(Modules.IHtmlModule)));
            Modules.Add(new Module("Url", typeof(Modules.IUrlModule)));
            Modules.Add(new Module("Slot", typeof(Modules.ISlotModule)));
        }

        #region Windsor IoC
        private IWindsorContainer _container;
        private void BootstrapContainer()
        {
            _container = new WindsorContainer()
                .Install(Castle.Windsor.Installer.FromAssembly.This());

            RegisterComponentsFromAssembly(Assembly.GetExecutingAssembly());

            _container.Register(Component.For<IKernel, IRootPath>().Instance(this));
        }

        protected void RegisterComponentsFromAssembly(Assembly assembly)
        {
            // TODO Verificare che questo metodo di inizializzazione e registrazione sia corretto
            _container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Service"))
                                .WithServiceAllInterfaces()
                                .LifestyleSingleton());

            // TODO E' corretto transient? Forse meglio usare scoped?
            // TODO Valutare se usare interfaccie per renderer e module
            _container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Renderer"))
                                .LifestyleTransient());
            _container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Module"))
                                .WithServiceAllInterfaces()
                                .WithServiceSelf()
                                .LifestyleTransient());
        }
        #endregion

        public string Path
        {
            get { return _workingPath; }
        }

        //public IContext Context
        //{
        //    get
        //    {
        //        // TODO Rivedere questa parte...gestire casi in cui non c'è un HttpContext 
        //        //  o quando ci sono chiamate nested al xrc e quindi il context è sempre lo stesso...
        //        if (System.Web.HttpContext.Current == null)
        //            throw new ApplicationException("HttpContext is null");

		//        var context = System.Web.HttpContext.Current.Items["XrcContext"] as IContext;
        //        return context;
        //    }
        //    private set
        //    {
        //        if (System.Web.HttpContext.Current == null)
        //            throw new ApplicationException("HttpContext is null");

        //        System.Web.HttpContext.Current.Items["XrcContext"] = value;
        //    }
        //}

        //public object GetModule(Type moduleType, object arguments)
        //{
        //    return _container.Resolve(moduleType, arguments);
        //}

        //public T GetModule<T>(object arguments)
        //{
        //    // TODO Devo fare il releasse dei moduli?
        //    return _container.Resolve<T>(arguments);
        //}

        private List<Module> _modules = new List<Module>();
        public List<Module> Modules
		{
			get { return _modules; }
		}

        #region Processing pipeline
        protected virtual void BeginRequest(IContext context)
        {
        }

        protected virtual void EndRequest(IContext context)
        {
        }

        private IRenderer CreateRenderer(Type type)
        {
            // TODO There are other way to create the components?
            // Should I release it?
            return (IRenderer)_container.Resolve(type);
        }

        public void RenderRequest(IContext context)
        {
			// TODO se lo user chiede http://localhost:8082/demowebsite/widgets
			// e widgets è una directory, devo fare il redirect su http://localhost:8082/demowebsite/widgets/ (con slash finale?)

            //Process pipeline

            BeginRequest(context);

            LoadContextModules(context);

            LoadConfiguration(context);

            if (!LocateXrcFile(context))
            {
                ProcessRequestNotFound(context);
                return;
            }

            LoadXrcDefinition(context);

            LoadParameters(context);

            var action = context.Page[context.Request.HttpMethod];
            if (action == null)
            {
                ProcessRequestNotFound(context);
                return;
            }

            var renderers = LoadRenderers(action, context.Modules.Values.ToArray());

            if (!string.IsNullOrWhiteSpace(action.Parent))
                RenderParent(context, action, renderers);
            else
            {
                foreach (var r in renderers)
                    r.Value.RenderRequest(context);
            }

            EndRequest(context);
        }

        private void RenderParent(IContext context, MashupAction action, Dictionary<string, IRenderer> renderers)
        {
			HttpResponseBase currentResponse = context.Response;

            // If a parent is defined call first it using the current response 
            // and defining a RenderSlot event that output the current slot inline.
            // The event will be called from the parent action by using Cms.Slot().
            // Parameters will be also copied from slot to parent.

            Uri parentUri = new Uri(context.Configuration.UrlContent(action.Parent, context.Request.Url));
            Context parentContext = new Context(new XrcRequest(parentUri), currentResponse);
            foreach (var item in context.Parameters)
                parentContext.Parameters.Add(item.Key, item.Value);
            parentContext.SlotCallback = (s, e) =>
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (XrcResponse response = new XrcResponse(stream))
                        {
                            context.Response = response;

                            IRenderer renderer;
                            if (renderers.TryGetValue(e.Name, out renderer))
                                renderer.RenderRequest(context);
                        }

                        // TODO Come gestire i casi di errore?

                        stream.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            e.Content = reader.ReadToEnd();
                        }
                    }
                };

            RenderRequest(parentContext);
        }

        private static void ProcessRequestNotFound(IContext context)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            context.Response.StatusDescription = string.Format("Resource '{0}' not found.", context.Request.Url);
        }

        private void LoadContextModules(IContext context)
        {
            // TODO Devo fare release? (potrei farla nella EndRequest)
            foreach (var m in Modules)
            {
                if (m.ModuleType == typeof(IKernel))
                    context.Modules.Add(m, this);
                else if (m.ModuleType == typeof(IContext))
                    context.Modules.Add(m, context);
                else
                {
                    object module = _container.Resolve(m.ModuleType, new { context = context });
                    context.Modules.Add(m, module);
                }
            }
        }

        private void LoadConfiguration(IContext context)
        {
            context.Configuration = _siteConfigurationProvider.GetSiteFromUri(context.Request.Url);
        }

        private bool LocateXrcFile(IContext context)
        {
			context.File = _fileLocator.Locate(context.Configuration.GetRelativeUri(context.Request.Url));
            if (context.File == null)
                return false;
            else
                return true;
        }

        private void LoadParameters(IContext context)
        {
            // Set context parameters
            // the order of the imported parameters define the parameters priority (that last overwrite)

            foreach (var item in context.Configuration.Parameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var item in context.File.UrlSegmentsParameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var item in context.Page.PageParameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var key in context.Request.QueryString.AllKeys)
                context.Parameters[key] = context.Request.QueryString[key];
        }

        private void LoadXrcDefinition(IContext context)
        {
            context.Page = _parser.Parse(context.File.FullPath, Modules.ToArray());
        }

        private Dictionary<string, IRenderer> LoadRenderers(MashupAction action, object[] modules)
        {
            Dictionary<string, IRenderer> renderers = new Dictionary<string, IRenderer>(StringComparer.OrdinalIgnoreCase);

            foreach (var rendererDefinition in action)
            {
                IRenderer renderer = CreateRenderer(rendererDefinition.RendererType);
                foreach (var property in rendererDefinition)
                {
                    object value;
                    if (property.Expression != null)
                        value = property.Expression.Eval(modules);
                    else
                        value = property.Value;

                    property.PropertyInfo.SetValue(renderer, value, null);
                }

                renderers.Add(rendererDefinition.Slot, renderer);
            }

            return renderers;
        }
        #endregion
    }
}
