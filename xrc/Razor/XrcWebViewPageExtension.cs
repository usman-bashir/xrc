using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using xrc.Razor;

namespace xrc.Razor
{
    class XrcWebViewPageExtension
    {
        const string VIEWDATA_XRCCONTEXT = "_XrcContext";
        const string VIEWDATA_IMODULEFACTORY = "_IModuleFactory";
        const string VIEWDATA_IMODULECATALOGSERVICE = "_IModuleCatalogService";

        private IContext _xrcContext;
        private Modules.IModuleFactory _moduleFactory;
        private Modules.IModuleCatalogService _moduleCatalog;
        private static readonly Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();

        public XrcWebViewPageExtension(WebViewPage page)
        {
            _xrcContext = (IContext)page.ViewData[VIEWDATA_XRCCONTEXT];
            _moduleFactory = (Modules.IModuleFactory)page.ViewData[VIEWDATA_IMODULEFACTORY];
            _moduleCatalog = (Modules.IModuleCatalogService)page.ViewData[VIEWDATA_IMODULECATALOGSERVICE];

            LoadModulesAndParameters(page);
        }

        private void LoadModulesAndParameters(WebViewPage page)
        {
            foreach (var p in page.GetType().GetProperties())
            {
                if (typeof(Modules.IModule).IsAssignableFrom(p.PropertyType))
                {
                    var component = _moduleCatalog.Get(p.Name);
                    if (p.PropertyType.IsAssignableFrom(component.Type))
                    {
                        var interceptor = new ModuleInterceptor(component, _xrcContext, _moduleFactory);
                        p.SetValue(page, _generator.CreateInterfaceProxyWithoutTarget(p.PropertyType, interceptor), null);
                    }
                }
                else
                {
                    ContextParameter parameter;
                    if (_xrcContext.Parameters.TryGetValue(p.Name, out parameter) &&
                                    p.PropertyType.IsAssignableFrom(parameter.Type))
                        p.SetValue(page, parameter.Value, null);
                }
            }
        }

        public static void LoadViewContextVariables(ViewContext viewContext, IContext context,
                                            Modules.IModuleCatalogService moduleCatalog, Modules.IModuleFactory moduleFactory)
        {
            viewContext.ViewData[Razor.XrcWebViewPageExtension.VIEWDATA_XRCCONTEXT] = context;
            viewContext.ViewData[Razor.XrcWebViewPageExtension.VIEWDATA_IMODULEFACTORY] = moduleFactory;
            viewContext.ViewData[Razor.XrcWebViewPageExtension.VIEWDATA_IMODULECATALOGSERVICE] = moduleCatalog;
        }
    }
}