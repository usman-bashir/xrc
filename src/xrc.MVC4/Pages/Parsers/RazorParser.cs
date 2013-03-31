using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Script;
using System.Xml.Linq;

namespace xrc.Pages.Parsers
{
	public class RazorParser : ResourceParserBase
	{
		readonly IViewCatalogService _viewCatalog;

		public RazorParser(IViewCatalogService viewCatalog)
			: base(".xrc.cshtml")
		{
			_viewCatalog = viewCatalog;
		}

		public override PageDefinition Parse(string resourceLocation)
		{
			var result = new PageDefinition();

			var action = new PageAction("GET");

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(RazorView).Name);
			if (viewComponentDefinition == null)
				throw new XrcException(string.Format("View '{0}' not found on catalog.", typeof(RazorView).Name));

			var view = new ViewDefinition(viewComponentDefinition, null);
			string propertyName = "ViewUrl";
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			string fileName = UriExtensions.GetName(resourceLocation);
			var propertyValue = new XValue(viewProperty.PropertyType, fileName);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}
	}
}
