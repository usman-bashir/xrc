﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Configuration;
using System.Reflection;
using xrc.Script;
using System.Globalization;
using xrc.Views;
using xrc.Modules;
using xrc.Pages.Script;

namespace xrc.Pages.Providers.FileSystem
{
    public class PageParserService : IPageParserService
    {
		private static XNamespace XMLNS = "urn:xrc";
		private static XName PAGE = XMLNS + "page";
		private static XName ACTION = XMLNS + "action";
        private static XName PARAMETERS = XMLNS + "parameters";
        private static XName ADD = XMLNS + "add";
        private static XName VIEW = XMLNS + "view";
		private static XName METHOD = "method";
        private static XName TYPE = "type";
        private static XName KEY = "key";
        private static XName VALUE = "value";
        private static XName PARENT = "parent";
        private static XName SLOT = "slot";
        private static XName ALLOWREQUESTOVERRIDE = "allowRequestOverride";
		private static string DEFAULT_METHOD = "GET";

        private const string MODULE_PREFIX = "xrc";

        private IPageScriptService _scriptService;
        private IModuleCatalogService _moduleCatalog;
        private IViewCatalogService _viewCatalog;

        public PageParserService(IPageScriptService scriptService, IModuleCatalogService moduleCatalog, IViewCatalogService viewCatalog)
        {
            _scriptService = scriptService;
            _moduleCatalog = moduleCatalog;
            _viewCatalog = viewCatalog;
        }

        // TODO Qui si può parsificare il file una sola volta e metterlo in cache (con dipendenza al file?)
		public PageParserResult Parse(string file)
        {
            try
            {
                // TODO Valutare se usare Xpath per la lettura
                XDocument xdoc = XDocument.Load(file);

				var page = new PageParserResult();

				var rootElement = xdoc.Element(PAGE);
				if (rootElement == null)
					throw new ApplicationException(string.Format("Element root '{0}' not found.", PAGE));

                ParseModules(xdoc, page);

                var paramsElement = xdoc.Root.Element(PARAMETERS);
                if (paramsElement != null)
                    ParseParameters(paramsElement, page);

                foreach (var actionElement in xdoc.Root.Elements(ACTION))
                {
                    string method = actionElement.AttributeAsOrDefault<string>(METHOD);
					if (method == null)
						method = DEFAULT_METHOD;
                    var action = new PageAction(method);
                    if (actionElement.Attribute(PARENT) != null)
                        action.Parent = actionElement.AttributeAs<string>(PARENT);

                    foreach (var viewElement in actionElement.Elements(VIEW))
                    {
                        var view = ParseView(viewElement, page);
                        action.Views.Add(view);
                    }

                    page.Actions.Add(action);
                }

                return page;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to parse '{0}'.", file), ex);
            }
        }

		private void ParseModules(XDocument doc, PageParserResult page)
        {
            foreach (var attr in doc.Root.Attributes())
            {
                if (attr.Name.Namespace == XNamespace.Xmlns)
                {
                    Uri moduleUri = new Uri(attr.Value);
                    if (moduleUri.Scheme == MODULE_PREFIX)
                    {
                        var moduleDefinition = new ModuleDefinition(attr.Name.LocalName, _moduleCatalog.Get(moduleUri.Segments[0]));
                        page.Modules.Add(moduleDefinition);
                    }
                }
            }
        }

		private void ParseParameters(XElement paramsElement, PageParserResult page)
        {
            foreach (var actionElement in paramsElement.Elements(ADD))
            {
                string key = actionElement.AttributeAs<string>(KEY);
                string value = actionElement.AttributeAsOrDefault<string>(VALUE);
                string typeName = actionElement.AttributeAsOrDefault<string>(TYPE);
                if (string.IsNullOrWhiteSpace(typeName))
                    typeName = typeof(string).FullName;
                Type type = Type.GetType(typeName, true, true);
                bool allowRequestOverride = actionElement.AttributeAsOrDefault<bool>(ALLOWREQUESTOVERRIDE);

                var xValue = _scriptService.Parse(value, type, page.Modules, page.Parameters);

                page.Parameters.Add(new PageParameter(key, xValue, allowRequestOverride)); ;
            }
        }

		private ViewDefinition ParseView(XElement xElement, PageParserResult page)
		{
			string typeName = xElement.AttributeAs<string>(TYPE);
            ComponentDefinition component = _viewCatalog.Get(typeName);
            if (component == null)
				throw new ApplicationException(string.Format("Component '{0}' not found'.", typeName));
			if (!typeof(IView).IsAssignableFrom(component.Type))
                throw new ApplicationException(string.Format("Type '{0}' is not a view.", component.Type));

            string slot = string.Empty;
            if (xElement.Attribute(SLOT) != null)
                slot = xElement.AttributeAs<string>(SLOT);

            ViewDefinition view = new ViewDefinition(component, slot);

            // TODO Escludere le property con un namespace? O usare un namespace particolare per le property?
            foreach (var element in xElement.Elements())
			{
                var property = ParseProperty(element, component.Type, page);

                view.Properties.Add(property);
			}

			return view;
		}

		private XProperty ParseProperty(XElement element, Type ownerType, PageParserResult page)
        {
            var property = ownerType.GetProperty(element.Name.LocalName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ApplicationException(string.Format("Property '{0}' not found on type '{0}.", element.Name.LocalName, ownerType));

            if (element.HasElements)
            {
                if (property.PropertyType == typeof(XDocument))
                {
					if (element.Elements().Count() > 1)
						throw new ApplicationException(string.Format("Element '{0}' not recognized for property '{1}', there is no root.", element.Name, property.Name));
					else
					{
						// TODO Valutare se c'è un modo migliore per passare da un XElement a un XDocument
						// o valutare se usare invece XElement direttamente e non richiedere un XDocument
						string xmlContent = element.Elements().First().ToString();
                        var xValue = new XValue(property.PropertyType, XDocument.Parse(xmlContent));
						return new XProperty(property, xValue);
					}
                }
                else
                    throw new ApplicationException(string.Format("Element '{0}' not recognized for property '{1}'", element.Name, property.Name));
            }
            else if (element.IsEmpty == false)
            {
                var xValue = _scriptService.Parse(element.Value, property.PropertyType, page.Modules, page.Parameters);
                return new XProperty(property, xValue);
            }
            else
                throw new ApplicationException(string.Format("Invalid element '{0}', valuenot defined.", element.Name));
        }
    }
}