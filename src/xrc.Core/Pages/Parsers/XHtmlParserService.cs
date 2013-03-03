using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Script;
using System.Xml.Linq;
using xrc.Pages.Providers;

namespace xrc.Pages.Parsers
{
	public class XHtmlParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;
        readonly IResourceProviderService _resourceProvider;

		public XHtmlParserService(IViewCatalogService viewCatalog,
								IResourceProviderService resourceProvider)
			: base(".xrc.xhtml")
		{
			_viewCatalog = viewCatalog;
            _resourceProvider = resourceProvider;
		}

		public override PageDefinition Parse(string resourceLocation)
		{
			var result = new PageDefinition();

			var action = new PageAction("GET");

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

            XDocument content = _resourceProvider.ResourceToXml(resourceLocation);
			var propertyValue = new XValue(viewProperty.PropertyType, content);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}
 	}
}
