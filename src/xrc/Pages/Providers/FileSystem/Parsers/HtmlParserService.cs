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

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public class HtmlParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;

		public HtmlParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog)
			: base(configParser, ".html")
		{
			_viewCatalog = viewCatalog;
		}

		// TODO E' possibile semplificare e irrobustire questo codice?
		// TODO Potrebero esserci problemi di cache e dipendenze? Da ottimizzare in qualche modo?

		protected override PageParserResult ParseFile(XrcFileResource fileResource)
		{
			var result = new PageParserResult();

			var action = CreateActionByConvention(fileResource);

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(HtmlView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);
			string propertyName = "Content";
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			string fullPath = fileResource.File.FullPath;
			var function = new Func<string>(() => File.ReadAllText(fullPath));
			var scriptExpression = new ScriptExpression("HtmlParserService_Expression", new ScriptParameterList(), function);
			var propertyValue = new XValue(viewProperty.PropertyType, scriptExpression);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}
	}
}
