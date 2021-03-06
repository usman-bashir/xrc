﻿using System;
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
using xrc.Pages.Providers;

namespace xrc.Views
{
    public class XsltView : IView
    {
        const string XSL_ARGUMENTS_NAMESPACE = "";
        const string XSL_EXTENSIONS_PREFIX = "xrc";
		readonly XsltSettings _settings = new XsltSettings();
        readonly IModuleFactory _moduleFactory;
		readonly IModuleCatalogService _moduleCatalog;
		readonly IResourceProviderService _resourceProvider;

		public XsltView(IModuleFactory moduleFactory, IModuleCatalogService moduleCatalog, IResourceProviderService resourceProvider)
        {
            _moduleFactory = moduleFactory;
            _moduleCatalog = moduleCatalog;
			_resourceProvider = resourceProvider;
        }

		public XDocument Xslt
        {
            get;
            set;
        }

		public string XsltFile
		{
			get;
			set;
		}

        public XDocument Data
        {
            get;
            set;
        }

		public string DataFile
		{
			get;
			set;
		}

        public void Execute(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

			if (Xslt == null && !string.IsNullOrEmpty(XsltFile))
				Xslt = _resourceProvider.ResourceToXml(context.Page.GetResourceLocation(XsltFile));
			if (Data == null && !string.IsNullOrEmpty(DataFile))
				Data = _resourceProvider.ResourceToXml(context.Page.GetResourceLocation(DataFile));

            XDocument transformData = Data;
            if (transformData == null)
                transformData = new XDocument(); 

            if (Xslt == null)
                throw new ArgumentNullException("Xslt");

			// TODO Valutare come fare cache di questa classe
            XslCompiledTransform transform = new XslCompiledTransform(); //Debug);

            XmlResolver resolver = new XmlUrlResolver();
            // TODO Valutare se CreateNavigator è sufficentemente performante e valutare anche il parameter XmlNameTable che potrebbe velocizzare la parsificazione...
            transform.Load(Xslt.CreateNavigator(), _settings, resolver);

            XsltArgumentList arguments = new XsltArgumentList();
            //arguments.XsltMessageEncountered += event  // per debug

            LoadParameters(context, arguments);

			var modules = new List<object>();
            try
            {
                LoadModulesFromNamespace(context, arguments, modules);

                Transform(context.Response.Output, transform, transformData, arguments);
            }
            finally
            {
                UnloadModules(modules);
            }
        }

        private static void LoadParameters(IContext context, XsltArgumentList arguments)
        {
			foreach (var item in context.Parameters)
			{
				if (item.Value != null)
					arguments.AddParam(SanitizeParamName(item.Name), XSL_ARGUMENTS_NAMESPACE, item.Value);
			}
        }

		private static string SanitizeParamName(string name)
		{
			return name.Replace(':', '_').Replace('.', '_').Replace('.', ',').Replace(" ", "");
		}

        private void UnloadModules(List<object> modules)
        {
            foreach (var m in modules)
                _moduleFactory.Release(m);
        }

		private void LoadModulesFromNamespace(IContext context, XsltArgumentList arguments, List<object> modules)
        {
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
        }

        private void Transform(System.IO.TextWriter response, XslCompiledTransform transform, XDocument transformData, XsltArgumentList arguments)
        {
            if (transform.OutputSettings.OutputMethod == XmlOutputMethod.Html)
            {
                // TODO Valutare se effettivamente serve usare un writer particolare
                // da questo sembrerebbe di si: http://stackoverflow.com/questions/887739/xslt-self-closing-tags-issue
                // ma in realtà per risolvere il problema dei self enclosing io ho dovuto togliere il namespace 
                // xmlns="http://www.w3.org/1999/xhtml" da gli xslt figli, e lasciarlo solo sul padre...non ho capito perchè
                using (XhtmlTextWriter htmlOutput = new XhtmlTextWriter(response))
                {
                    transform.Transform(transformData.CreateNavigator(), arguments, htmlOutput);
                }
            }
            else
                transform.Transform(transformData.CreateNavigator(), arguments, response);


            // TODO Should I need to set content type or other response properties?
            // maybe using some of the properties of the transform.OutputSettings...
        }
    }
}
