﻿using System;
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
	public class HtmlParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;
		readonly IPageProviderService _pageProvider;

		public HtmlParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog,
								IPageProviderService pageProvider)
			: base(configParser, ".xrc.html")
		{
			_viewCatalog = viewCatalog;
			_pageProvider = pageProvider;
		}

		// TODO E' possibile semplificare e irrobustire questo codice?
		// TODO Potrebero esserci problemi di cache e dipendenze? Da ottimizzare in qualche modo?

		protected override PageParserResult ParseFile(XrcItem file)
		{
			var result = new PageParserResult();

			var action = new PageAction("GET");
			action.Layout = GetDefaultLayoutByConvention(file);

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(HtmlView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);
			string propertyName = "Content";
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			string content = _pageProvider.ResourceToHtml(file.ResourceLocation);
			var propertyValue = new XValue(viewProperty.PropertyType, content);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}
	}
}
