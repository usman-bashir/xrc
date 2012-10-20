using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Script;
using System.Xml.Linq;
using System.IO;

namespace xrc.Pages.Providers.Common.Parsers
{
	public class XsltParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;
		readonly IPageProviderService _pageProvider;

		public XsltParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog,
								IPageProviderService pageProvider)
			: base(configParser, ".xrc.xslt")
		{
			_viewCatalog = viewCatalog;
			_pageProvider = pageProvider;
		}

		// TODO E' possibile semplificare e irrobustire questo codice?
		// TODO Potrebero esserci problemi di cache e dipendenze? Da ottimizzare in qualche modo?

		protected override PageParserResult ParseFile(XrcItem item)
		{
			var result = new PageParserResult();

			var action = new PageAction("GET");
			action.Layout = GetDefaultLayoutByConvention(item);

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(XsltView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);

			XDocument xsltContent = _pageProvider.ResourceToXml(item.ResourceLocation);
			AddProperty(viewComponentDefinition, view, "Xslt", xsltContent);

			string dataVirtualPath = item.ResourceLocation.Replace(".xrc.xslt", ".xml");
			if (_pageProvider.ResourceExists(dataVirtualPath))
			{
				AddProperty(viewComponentDefinition, view, "Data", _pageProvider.ResourceToXml(dataVirtualPath));
			}

			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}

		private static void AddProperty(ComponentDefinition viewComponentDefinition, ViewDefinition view, string propertyName, object propertyValue)
		{
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			var propertyXValue = new XValue(viewProperty.PropertyType, propertyValue);

			view.Properties.Add(new XProperty(viewProperty, propertyXValue));
		}
	}
}
