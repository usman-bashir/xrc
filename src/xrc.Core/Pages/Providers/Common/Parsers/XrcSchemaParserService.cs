using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Views;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace xrc.Pages.Providers.Common.Parsers
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
		readonly static XName LAYOUT = "layout";
		readonly static XName SLOT = "slot";
		readonly static XName URL = "url";
		readonly static XName OUTPUTCACHE = XMLNS + "outputcache";
		readonly static XName CATCHEXCEPTION = XMLNS + "catchException";
		readonly static XName ALLOWREQUESTOVERRIDE = "allowRequestOverride";
		readonly static string DEFAULT_METHOD = "GET";

        const string MODULE_PREFIX = "xrc";

		readonly IResourceProviderService _resourceProvider;
        readonly IPageScriptService _scriptService;
        readonly IModuleCatalogService _moduleCatalog;
        readonly IViewCatalogService _viewCatalog;

		public XrcSchemaParserService(IResourceProviderService resourceProvider,
									IPageScriptService scriptService, 
									IModuleCatalogService moduleCatalog, 
									IViewCatalogService viewCatalog)
		{
			_resourceProvider = resourceProvider;
			_scriptService = scriptService;
			_moduleCatalog = moduleCatalog;
			_viewCatalog = viewCatalog;
		}

		public PageParserResult Parse(XrcItem item)
		{
			var result = new PageParserResult();
			try
			{
				// TODO Valutare se usare Xpath per la lettura

				XDocument xdoc = _resourceProvider.ResourceToXml(item.ResourceLocation);

				var rootElement = xdoc.Element(PAGE);
				if (rootElement == null)
					throw new ApplicationException(string.Format("Element root '{0}' not found.", PAGE));

				ParseModules(xdoc, result);

				var paramsElement = xdoc.Root.Element(PARAMETERS);
				if (paramsElement != null)
					ParseParameters(paramsElement, result);

				ParseActions(item, xdoc, result);
			}
			catch (Exception ex)
			{
				throw new XrcException(string.Format("Failed to parse '{0}'.", item.ResourceLocation), ex);
			}

			return result;
		}

		private void ParseActions(XrcItem item, XDocument xdoc, PageParserResult parserResult)
		{
			foreach (var actionElement in xdoc.Root.Elements(ACTION))
			{
				string method = actionElement.AttributeAsOrDefault<string>(METHOD);
				if (method == null)
					method = DEFAULT_METHOD;
				var action = new PageAction(method);
				if (actionElement.Attribute(LAYOUT) != null)
					action.Layout = actionElement.AttributeAs<string>(LAYOUT);

				foreach (var subElement in actionElement.Elements())
				{
					if (subElement.Name.Namespace != XMLNS)
						continue;

					if (subElement.Name == OUTPUTCACHE)
					{
					}
					else if (subElement.Name == CATCHEXCEPTION)
					{
						var url = subElement.AttributeAsOrDefault<string>(URL);
						action.CatchException = new PageCatchException(url);
					}
					else
					{
						var view = ParseView(item, subElement, parserResult);
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
			foreach (var addElement in paramsElement.Elements(ADD))
			{
				string key = addElement.AttributeAs<string>(KEY);
				string value = addElement.AttributeAsOrDefault<string>(VALUE);
				string typeName = addElement.AttributeAsOrDefault<string>(TYPE);
				if (string.IsNullOrWhiteSpace(typeName))
					typeName = typeof(string).FullName;
				Type type = Type.GetType(typeName, true, true);
				bool allowRequestOverride = addElement.AttributeAsOrDefault<bool>(ALLOWREQUESTOVERRIDE);

				var xValue = _scriptService.Parse(value, type, parserResult.Modules, parserResult.Parameters);

				parserResult.Parameters.Add(new PageParameter(key, xValue, allowRequestOverride));
			}
		}

		private ViewDefinition ParseView(XrcItem item, XElement xElement, PageParserResult parserResult)
		{
			string viewTypeName = xElement.Name.LocalName;
			ComponentDefinition viewComponent = _viewCatalog.Get(viewTypeName);
			if (viewComponent == null)
				throw new XrcException(string.Format("View '{0}' not found.", viewTypeName));

			if (!typeof(IView).IsAssignableFrom(viewComponent.Type))
				throw new ApplicationException(string.Format("Type '{0}' is not a view.", viewComponent.Type));

			string slot = string.Empty;
			if (xElement.Attribute(SLOT) != null)
				slot = xElement.AttributeAs<string>(SLOT);

			ViewDefinition view = new ViewDefinition(viewComponent, slot);

			// TODO Escludere le property con un namespace? O usare un namespace particolare per le property?
			foreach (var element in xElement.Elements())
			{
				var property = ParseProperty(item, element, viewComponent.Type, parserResult);

				view.Properties.Add(property);
			}

			return view;
		}

		private XProperty ParseProperty(XrcItem item, XElement element, Type ownerType, PageParserResult parserResult)
		{
			string propertyName = element.Name.LocalName;
			var property = ownerType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			if (property == null)
			{
				//XProperty propertyFile = ParsePropertyFile(item, ownerType, propertyName, element.Value);
				//if (propertyFile != null)
				//    return propertyFile;

				throw new ApplicationException(string.Format("Property '{0}' not found on type '{1}.", propertyName, ownerType));
			}

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
				throw new ApplicationException(string.Format("Invalid element '{0}', value not defined.", element.Name));
		}

		//private XProperty ParsePropertyFile(XrcItem item, Type ownerType, string propertyName, string value)
		//{
		//    const string FILE_INCLUDE_SUFFIX = "File";
		//    if (propertyName.EndsWith(FILE_INCLUDE_SUFFIX, StringComparison.OrdinalIgnoreCase))
		//    {
		//        string propertyNameBase = propertyName.Substring(0, propertyName.Length - FILE_INCLUDE_SUFFIX.Length);
		//        var propertyBase = ownerType.GetProperty(propertyNameBase, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
		//        if (propertyBase != null)
		//        {
		//            // TODO Quando e se si metterà in cache il file xrc con dipendenza al file stesso ricordarsi di considerare anche questi file (letti inline per le property).

		//            string resourceLocation = UriExtensions.BuildVirtualPath(item.ResourceLocation, value);
		//            if (propertyBase.PropertyType == typeof(XDocument))
		//            {
		//                var xValue = new XValue(propertyBase.PropertyType, _resourceProvider.ResourceToXml(resourceLocation));
		//                return new XProperty(propertyBase, xValue);
		//            }
		//            else if (propertyBase.PropertyType == typeof(string))
		//            {
		//                var xValue = new XValue(propertyBase.PropertyType, _resourceProvider.ResourceToText(resourceLocation));
		//                return new XProperty(propertyBase, xValue);
		//            }
		//            else if (propertyBase.PropertyType == typeof(byte[]))
		//            {
		//                var xValue = new XValue(propertyBase.PropertyType, _resourceProvider.ResourceToBytes(resourceLocation));
		//                return new XProperty(propertyBase, xValue);
		//            }
		//        }
		//    }
		//    return null;
		//}
	}
}
