using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Configuration;
using System.Reflection;
using xrc.Script;
using System.Globalization;
using xrc.Renderers;
using xrc.Modules;

namespace xrc.SiteManager
{
    public class MashupParserService : IMashupParserService
    {
		private static XNamespace XMLNS = "urn:xrc";
		private static XName PAGE = XMLNS + "page";
		private static XName ACTION = XMLNS + "action";
        private static XName PARAMETERS = XMLNS + "parameters";
        private static XName ADD = XMLNS + "add";
        private static XName RENDERER = XMLNS + "renderer";
		private static XName METHOD = "method";
        private static XName TYPE = "type";
        private static XName KEY = "key";
        private static XName VALUE = "value";
        private static XName PARENT = "parent";
        private static XName SLOT = "slot";
        private static XName ALLOWREQUESTOVERRIDE = "allowRequestOverride";

        private const string MODULE_PREFIX = "xrc";

        private IMashupScriptService _scriptService;
        private IModuleCatalogService _moduleCatalog;
        private IRendererCatalogService _rendererCatalog;

        public MashupParserService(IMashupScriptService scriptService, IModuleCatalogService moduleCatalog, IRendererCatalogService rendererCatalog)
        {
            _scriptService = scriptService;
            _moduleCatalog = moduleCatalog;
            _rendererCatalog = rendererCatalog;
        }

        // TODO Qui si può parsificare il file una sola volta e metterlo in cache (con dipendenza al file?)
        public MashupPage Parse(string file)
        {
            try
            {
                // TODO Valutare se usare Xpath per la lettura
                XDocument xdoc = XDocument.Load(file);

                MashupPage page = new MashupPage();

				var rootElement = xdoc.Element(PAGE);
				if (rootElement == null)
					throw new ApplicationException(string.Format("Element root '{0}' not found.", PAGE));

                ParseModules(xdoc, page);

                var paramsElement = xdoc.Root.Element(PARAMETERS);
                if (paramsElement != null)
                    ParseParameters(paramsElement, page);

                foreach (var actionElement in xdoc.Root.Elements(ACTION))
                {
                    string method = actionElement.AttributeAs<string>(METHOD);
                    MashupAction action = new MashupAction(method);
                    if (actionElement.Attribute(PARENT) != null)
                        action.Parent = actionElement.AttributeAs<string>(PARENT);

                    foreach (var rendererElement in actionElement.Elements(RENDERER))
                    {
                        var renderer = ParseRenderer(rendererElement, page);
                        action.Renderers.Add(renderer);
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

        private void ParseModules(XDocument doc, MashupPage page)
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

        private void ParseParameters(XElement paramsElement, MashupPage page)
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

                page.Parameters.Add(new MashupParameter(key, xValue, allowRequestOverride)); ;
            }
        }

        private RendererDefinition ParseRenderer(XElement xElement, MashupPage page)
		{
			string typeName = xElement.AttributeAs<string>(TYPE);
            ComponentDefinition component = _rendererCatalog.Get(typeName);
            if (component == null)
				throw new ApplicationException(string.Format("Component '{0}' not found'.", typeName));
			if (!typeof(IRenderer).IsAssignableFrom(component.Type))
                throw new ApplicationException(string.Format("Type '{0}' is not a renderer.", component.Type));

            string slot = string.Empty;
            if (xElement.Attribute(SLOT) != null)
                slot = xElement.AttributeAs<string>(SLOT);

            RendererDefinition renderer = new RendererDefinition(component, slot);

            // TODO Escludere le property con un namespace? O usare un namespace particolare per le property?
            foreach (var element in xElement.Elements())
			{
                var property = ParseProperty(element, component.Type, page);

                renderer.Properties.Add(property);
			}

			return renderer;
		}

        private XProperty ParseProperty(XElement element, Type ownerType, MashupPage page)
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
