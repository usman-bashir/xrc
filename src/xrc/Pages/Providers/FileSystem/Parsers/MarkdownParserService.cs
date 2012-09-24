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
	public class MarkdownParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;

		public MarkdownParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog)
			: base(configParser, ".md")
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

			var viewComponentDefinition = _viewCatalog.Get(typeof(MarkdownView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);
			string propertyName = "Content";
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			string content = File.ReadAllText(fileResource.File.FullPath);
			var propertyValue = new XValue(viewProperty.PropertyType, content);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}
	}
}
