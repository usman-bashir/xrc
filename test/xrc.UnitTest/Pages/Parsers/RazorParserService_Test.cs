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

namespace xrc.Pages.Parsers
{
	[TestClass]
	public class RazorParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_razor()
		{
			var resourceLocation = "item.xrc.cshtml";
			var viewType = typeof(RazorView);

			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new RazorParser(viewCatalog);

            PageDefinition page = target.Parse(resourceLocation);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["ViewUrl"].Value.Value;
			Assert.AreEqual("item.xrc.cshtml", content);
		}
    }
}
