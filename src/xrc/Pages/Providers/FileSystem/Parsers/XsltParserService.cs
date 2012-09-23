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

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public class XsltParserService : ParserServiceBase
	{
		readonly IViewCatalogService _viewCatalog;

		public XsltParserService(IXrcSchemaParserService configParser,
								IViewCatalogService viewCatalog)
			: base(configParser, ".xslt")
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

			var viewComponentDefinition = _viewCatalog.Get(typeof(XsltView).Name);
			var view = new ViewDefinition(viewComponentDefinition, null);

			string xsltFullPath = fileResource.File.FullPath;
			var xsltFunction = new Func<XDocument>(() => XDocument.Load(xsltFullPath));
			AddProperty(viewComponentDefinition, view, "Xslt", xsltFunction);

			string dataFullPath = fileResource.File.FullPath.Replace(".xrc.xslt", ".xml");
			if (File.Exists(dataFullPath))
			{
				var dataFunction = new Func<XDocument>(() => XDocument.Load(dataFullPath));
				AddProperty(viewComponentDefinition, view, "Data", dataFunction);
			}

			action.Views.Add(view);
			result.Actions.Add(action);

			return result;
		}

		private static void AddProperty(ComponentDefinition viewComponentDefinition, ViewDefinition view, string propertyName, Func<XDocument> function)
		{
			var viewProperty = viewComponentDefinition.Type.GetProperty(propertyName);
			if (viewProperty == null)
				throw new XrcException(string.Format("Property '{0}' for type '{1}' not found.", propertyName, viewComponentDefinition.Type.FullName));

			var scriptExpression = new ScriptExpression("XsltParserService_Expression", new ScriptParameterList(), function);
			var propertyValue = new XValue(viewProperty.PropertyType, scriptExpression);

			view.Properties.Add(new XProperty(viewProperty, propertyValue));
		}
	}
}
