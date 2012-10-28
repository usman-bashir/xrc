using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xrc.Views;
using xrc.Script;
using Moq;
using System.Xml.Linq;
using System.Xml.XPath;
using xrc.Modules;
using System.IO;

namespace xrc.Pages.Providers.Common.Parsers
{
	[TestClass]
	public class RazorParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_razor()
		{
			var file = GetItem("item.xrc.cshtml");
			var viewType = typeof(RazorView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new RazorParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["ViewUrl"].Value.Value;
			Assert.AreEqual("item.xrc.cshtml", content);
		}

		private XrcItem GetItem(string fileName)
		{
			var item = XrcItem.NewXrcFile(fileName);
			var xrcRoot = XrcItem.NewRoot("~", item);

			return item;
		}
    }
}
