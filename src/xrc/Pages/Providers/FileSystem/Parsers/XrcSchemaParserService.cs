using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Views;
using System.Xml.Linq;
using System.Reflection;

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public class XrcSchemaParserService : IXrcSchemaParserService
	{
		readonly static XNamespace XMLNS = "urn:xrc";
		readonly static XName PAGE = XMLNS + "page";
		readonly static XName ACTION = XMLNS + "action";
		readonly static XName PARAMETERS = XMLNS + "parameters";
		readonly static XName ADD = XMLNS + "add";
		readonly static XName METHOD = "method";
		readonly static XName TYPE = "type";
		readonly static XName KEY = "key";
		readonly static XName VALUE = "value";
		readonly static XName PARENT = "parent";
		readonly static XName SLOT = "slot";
		readonly static XName OUTPUTCACHE = XMLNS + "outputcache";
		readonly static XName ALLOWREQUESTOVERRIDE = "allowRequestOverride";
		readonly static string DEFAULT_METHOD = "GET";

        const string MODULE_PREFIX = "xrc";

        readonly IPageScriptService _scriptService;
        readonly IModuleCatalogService _moduleCatalog;
        readonly IViewCatalogService _viewCatalog;

		public XrcSchemaParserService(IPageScriptService scriptService, IModuleCatalogService moduleCatalog, IViewCatalogService viewCatalog)
		{
			_scriptService = scriptService;
			_moduleCatalog = moduleCatalog;
			_viewCatalog = viewCatalog;
		}

		public PageParserResult Parse(string fullpath)
		{
			var result = new PageParserResult();
			try
			{
				// TODO Valutare se usare Xpath per la lettura
				XDocument xdoc = XDocument.Load(fullpath);

				var rootElement = xdoc.Element(PAGE);
				if (rootElement == null)
					throw new ApplicationException(string.Format("Element root '{0}' not found.", PAGE));

				ParseModules(xdoc, result);

				var paramsElement = xdoc.Root.Element(PARAMETERS);
				if (paramsElement != null)
					ParseParameters(paramsElement, result);

				ParseActions(xdoc, result);
			}
			catch (Exception ex)
			{
				throw new XrcException(string.Format("Failed to parse '{0}'.", fullpath), ex);
			}

			return result;
		}

		private void ParseActions(XDocument xdoc, PageParserResult parserResult)
		{
			foreach (var actionElement in xdoc.Root.Elements(ACTION))
			{
				string method = actionElement.AttributeAsOrDefault<string>(METHOD);
				if (method == null)
					method = DEFAULT_METHOD;
				var action = new PageAction(method);
				if (actionElement.Attribute(PARENT) != null)
					action.Parent = actionElement.AttributeAs<string>(PARENT);

				foreach (var viewElement in actionElement.Elements())
				{
					if (viewElement.Name != OUTPUTCACHE)
					{
						var view = ParseView(viewElement, parserResult);
						action.Views.Add(view);
					}
				}

				parserResult.Actions.Add(action);
			}
		}

		private void ParseModules(XDocument doc, PageParserResult parserResult)
		{
			foreach (var attr in doc.Root.Attributes())
			{
				if (attr.Name.Namespace == XNamespace.Xmlns)
				{
					Uri moduleUri = new Uri(attr.Value);
					if (moduleUri.Scheme == MODULE_PREFIX)
					{
						string name = attr.Name.LocalName;

						var moduleDefinition = new ModuleDefinition(name, _moduleCatalog.Get(moduleUri.Segments[0]));
						parserResult.Modules.Add(moduleDefinition);
					}
				}
			}
		}

		private void ParseParameters(XElement paramsElement, PageParserResult parserResult)
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

				var xValue = _scriptService.Parse(value, type, parserResult.Modules, parserResult.Parameters);

				parserResult.Parameters.Add(new PageParameter(key, xValue, allowRequestOverride));
			}
		}

		private ViewDefinition ParseView(XElement xElement, PageParserResult parserResult)
		{
			string typeName = xElement.Name.LocalName;
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
				var property = ParseProperty(element, component.Type, parserResult);

				view.Properties.Add(property);
			}

			return view;
		}

		private XProperty ParseProperty(XElement element, Type ownerType, PageParserResult parserResult)
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
				var xValue = _scriptService.Parse(element.Value, property.PropertyType, parserResult.Modules, parserResult.Parameters);
				return new XProperty(property, xValue);
			}
			else
				throw new ApplicationException(string.Format("Invalid element '{0}', valuenot defined.", element.Name));
		}
	}
}
