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
using xrc.Pages.Providers;

namespace xrc.Pages.Parsers
{
	public class XsltParser : ResourceParserBase
	{
		readonly IViewCatalogService _viewCatalog;
		readonly IResourceProviderService _resourceProvider;

		public XsltParser(IViewCatalogService viewCatalog,
								IResourceProviderService resourceProvider)
			: base(".xrc.xslt")
		{
			_viewCatalog = viewCatalog;
			_resourceProvider = resourceProvider;
		}

		// TODO E' possibile semplificare e irrobustire questo codice?
		// TODO Potrebero esserci problemi di cache e dipendenze? Da ottimizzare in qualche modo?

		public override PageDefinition Parse(string resourceLocation)
		{
			var result = new PageDefinition();

			var action = new PageAction("GET");

			var moduleDefinitionList = new ModuleDefinitionList();
			var pageParameters = new PageParameterList();

			var viewComponentDefinition = _viewCatalog.Get(typeof(XsltView).Name);
			if (viewComponentDefinition == null)
				throw new XrcException(string.Format("View '{0}' not found on catalog.", typeof(XsltView).Name));

			var view = new ViewDefinition(viewComponentDefinition, null);

            XDocument xsltContent = _resourceProvider.ResourceToXml(resourceLocation);
			AddProperty(viewComponentDefinition, view, "Xslt", xsltContent);

            string dataVirtualPath = resourceLocation.Replace(".xrc.xslt", ".xml");
			if (_resourceProvider.ResourceExists(dataVirtualPath))
			{
				AddProperty(viewComponentDefinition, view, "Data", _resourceProvider.ResourceToXml(dataVirtualPath));
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
