using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Script;
using System.Xml.Linq;

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public class XHtmlParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;

		public XHtmlParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog)
			: base(configParser, ".xhtml")
		{
			_viewCatalog = viewCatalog;
		}

		// TODO E' possibile semplificare e irrobustire questo codice?

		protected override PageParserResult ParseFile(XrcFileResource fileResource)
		{
			var result = new PageParserResult();

			var action = new PageAction("GET");

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(XHtmlView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);
			var viewProperty = viewComponentDefinition.Type.GetProperty("Content");

			string fullPath = fileResource.File.FullPath;
			var function = new Func<XDocument>(() => XDocument.Load(fullPath));
			var scriptExpression = new ScriptExpression("XHtmlParserService_Expression", new ScriptParameterList(), function);
			var propertyValue = new XValue(viewProperty.PropertyType, scriptExpression);

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
