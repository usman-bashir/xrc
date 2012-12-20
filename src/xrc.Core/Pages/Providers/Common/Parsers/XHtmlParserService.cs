﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Script;
using System.Xml.Linq;

namespace xrc.Pages.Providers.Common.Parsers
{
	public class XHtmlParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;
		readonly IResourceProviderService _resourceProvider;

		public XHtmlParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog,
								IResourceProviderService resourceProvider)
			: base(configParser, ".xrc.xhtml")
		{
			_viewCatalog = viewCatalog;
			_resourceProvider = resourceProvider;
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

			var viewComponentDefinition = _viewCatalog.Get(typeof(XHtmlView).Name);
			if (viewComponentDefinition == null)
				throw new XrcException(string.Format("View '{0}' not found on catalog.", typeof(XHtmlView).Name));

			var view = new ViewDefinition(viewComponentDefinition, null);
			string propertyName = "Content";
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			XDocument content = _resourceProvider.ResourceToXml(file.ResourceLocation);
			var propertyValue = new XValue(viewProperty.PropertyType, content);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}

		// Version che utilizza lo scriptService per leggere il file
		//protected override PageParserResult ParseFile(XrcFileResource fileResource)
		//{
		//    var result = new PageParserResult();

		//    var action = new PageAction("GET");

		//    var moduleDefinitionList = new ModuleDefinitionList();
		//    var fileModule = _moduleCatalog.Get(typeof(FileModule).Name);
		//    var fileModuleDefinition = new ModuleDefinition(fileModule.Name, fileModule);
		//    moduleDefinitionList.Add(fileModuleDefinition);

		//    var pageParameters = new PageParameterList();

		//    var viewComponentDefinition = _viewCatalog.Get(typeof(XHtmlView).Name);
		//    var view = new ViewDefinition(viewComponentDefinition, null);
		//    var viewProperty = viewComponentDefinition.Type.GetProperty("Content");

		//    var expression = string.Format("@{0}.Xml(\"{1}\")", fileModule.Name, fileResource.File.FileName);
		//    var propertyValue = _scriptService.Parse(expression, viewProperty.PropertyType, moduleDefinitionList, pageParameters);

		//    view.Properties.Add(new XProperty(viewProperty, propertyValue));
		//    action.Views.Add(view);
		//    result.Actions.Add(action);
		//    result.Modules.Add(fileModuleDefinition);

		//    return result;
		//}
	}
}