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

namespace xrc.SiteManager
{
    public class MashupParserService : IMashupParserService
    {
		private static XNamespace XMLNS = "urn:xrc";
		private static XName XRC = XMLNS + "xrc";
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

        private IScriptService _scriptService;

		public MashupParserService(IScriptService scriptService)
        {
            _scriptService = scriptService;
        }

        // TODO Qui si può parsificare il file una solva volta e metterlo in cache (con dipendenza al file?)
        public MashupPage Parse(string file, Module[] modules)
        {
            try
            {
                XDocument xdoc = XDocument.Load(file);

                MashupPage page = new MashupPage();

				var rootElement = xdoc.Element(XRC);
				if (rootElement == null)
					throw new ApplicationException(string.Format("Element root '{0}' not found.", XRC));

                // TODO Valutare se usare Xpath per la lettura

                foreach (var actionElement in xdoc.Root.Elements(ACTION))
                {
                    string method = actionElement.AttributeAs<string>(METHOD);
                    MashupAction action = new MashupAction(method);
                    if (actionElement.Attribute(PARENT) != null)
                        action.Parent = actionElement.AttributeAs<string>(PARENT);

                    foreach (var rendererElement in actionElement.Elements(RENDERER))
                    {
                        var renderer = ParseRenderer(rendererElement, modules);
                        action.Add(renderer);
                    }

                    page.Add(action);
                }

                var paramsElement = xdoc.Root.Element(PARAMETERS);
                if (paramsElement != null)
                    ParseParameters(paramsElement, page);

                return page;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to parse '{0}'.", file), ex);
            }
        }

        private void ParseParameters(XElement paramsElement, MashupPage page)
        {
            foreach (var actionElement in paramsElement.Elements(ADD))
            {
                string key = actionElement.AttributeAs<string>(KEY);
                string value = actionElement.AttributeAs<string>(VALUE);
                page.PageParameters.Add(key, value);
            }
        }

        private RendererDefinition ParseRenderer(XElement xElement, Module[] modules)
		{
			string typeName = xElement.AttributeAs<string>(TYPE);
			Type type = TypeResolverService.ResolveType(typeName);
			if (type == null)
				throw new ApplicationException(string.Format("Type '{0}' not found'.", typeName));
			if (!typeof(IRenderer).IsAssignableFrom(type))
				throw new ApplicationException(string.Format("Type '{0}' is not a renderer.", type));

            string slot = string.Empty;
            if (xElement.Attribute(SLOT) != null)
                slot = xElement.AttributeAs<string>(SLOT);

            RendererDefinition renderer = new RendererDefinition(type, slot);

            // TODO Escludere le property con un namespace? O usare un namespace particolare per le property?
            foreach (var element in xElement.Elements())
			{
                var property = ParseProperty(element, type, modules);

                renderer.Add(property);
			}

			return renderer;
		}

        private XProperty ParseProperty(XElement element, Type ownerType, Module[] modules)
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
						return new XProperty(property, XDocument.Parse(xmlContent));
					}
                }
                else
                    throw new ApplicationException(string.Format("Element '{0}' not recognized for property '{1}'", element.Name, property.Name));
            }
            else if (element.IsEmpty == false)
            {
                string script;
                if (_scriptService.TryExtractInlineScript(element.Value, out script))
                {
                    Dictionary<string, Type> arguments = new Dictionary<string, Type>();
                    foreach (var m in modules)
                        arguments.Add(m.Name, m.ModuleType);

                    IScriptExpression expression = _scriptService.Parse(script, arguments, property.PropertyType);
                    return new XProperty(property, expression);
                }
                else
                {
                    object value = ConvertEx.ChangeType(element.Value, property.PropertyType, CultureInfo.InvariantCulture);
                    return new XProperty(property, value);
                }
            }
            else
                throw new ApplicationException(string.Format("Invalid element '{0}', valuenot defined.", element.Name));
        }
    }
}
