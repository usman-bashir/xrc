using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Web.UI;
using xrc.Modules;

namespace xrc.Renderers
{
    public class XsltRenderer : IRenderer
    {
        public static ComponentDefinition Definition = new ComponentDefinition("XsltRenderer", typeof(XsltRenderer));

        private const string XSL_ARGUMENTS_NAMESPACE = "";
        private const string XSL_EXTENSIONS_PREFIX = "xrc";
        private XslCompiledTransform _transform;
        private XsltSettings _settings = new XsltSettings();
        private IModuleFactory _moduleFactory;
        private IModuleCatalogService _moduleCatalog;

        public XsltRenderer(IModuleFactory moduleFactory, IModuleCatalogService moduleCatalog)
        {
            //this.Debug = false;
            this.Xslt = null;
            _moduleFactory = moduleFactory;
            _moduleCatalog = moduleCatalog;
        }

		//public bool Debug
		//{
		//    get;
		//    set;
		//}

		public XDocument Xslt
        {
            get;
            set;
        }
		//public bool EnableDocumentFunction 
		//{
		//    get { return _settings.EnableDocumentFunction;}
		//    set { _settings.EnableDocumentFunction = value; }
		//}
		//public bool EnableScript 
		//{
		//    get { return _settings.EnableScript; }
		//    set { _settings.EnableScript = value; }
		//}
        public XDocument Data
        {
            get;
            set;
        }

        public void RenderRequest(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            XDocument transformData = Data;
            if (transformData == null)
                transformData = new XDocument(); 

            if (Xslt == null)
                throw new ArgumentNullException("Xslt");

			// TODO Valutare se fare cache di questa classe
            _transform = new XslCompiledTransform(); //Debug);

			// TODO Valutare se CreateNavigator è sufficentemente performance e valutare anche il parameter XmlNameTable che potrebbe velocizzare la parsificazione...

            XmlResolver resolver = new XmlUrlResolver();
            _transform.Load(Xslt.CreateNavigator(), _settings, resolver);

            XsltArgumentList arguments = new XsltArgumentList();
            //arguments.XsltMessageEncountered += event

            foreach (var item in context.Parameters)
                arguments.AddParam(item.Key, XSL_ARGUMENTS_NAMESPACE, item.Value);

            // Load modules from xsl namespaces
            List<object> modules = new List<object>();
            foreach (var attr in Xslt.Root.Attributes())
            {
                if (attr.Name.Namespace == XNamespace.Xmlns)
                {
                    Uri moduleUri = new Uri(attr.Value);
                    if (moduleUri.Scheme == XSL_EXTENSIONS_PREFIX)
                    {
                        var moduleDefinition = _moduleCatalog.Get(moduleUri.Segments[0]);
                        var module = _moduleFactory.Get(moduleDefinition, context);
                        modules.Add(module);
                        arguments.AddExtensionObject(attr.Value, module);
                    }
                }
            }

			if (_transform.OutputSettings.OutputMethod == XmlOutputMethod.Html)
			{
				// TODO Valutare se effettivamente serve usare un writer particolare
				// da questo sembrerebbe di si: http://stackoverflow.com/questions/887739/xslt-self-closing-tags-issue
				// ma in realtà per risolvere il problema dei self enclosing io ho dovuto togliere il namespace 
				// xmlns="http://www.w3.org/1999/xhtml" da gli xslt figli, e lasciarlo solo sul padre...non ho capito perchè
				using (XhtmlTextWriter htmlOutput = new XhtmlTextWriter(context.Response.Output))
				{
					_transform.Transform(transformData.CreateNavigator(), arguments, htmlOutput);
				}
			}
			else
				_transform.Transform(transformData.CreateNavigator(), arguments, context.Response.Output);

            // TODO Should I need to set content type or other response properties?
			// maybe using some of the properties of the _transform.OutputSettings
        }
    }
}
