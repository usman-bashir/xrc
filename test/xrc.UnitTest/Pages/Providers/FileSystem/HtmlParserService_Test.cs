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
using xrc.Pages.Providers.FileSystem.Parsers;
using xrc.Modules;

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
	public class HtmlParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_html()
		{
			var file = GetFile(@"sampleWebSite2\conventions_html.xrc.html");
			var viewType = typeof(HtmlView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new HtmlParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			Assert.AreEqual(typeof(string), view.Properties["Content"].Value.Expression.ReturnType);
			ScriptExpression expression = (ScriptExpression)view.Properties["Content"].Value.Expression;

			var content = (string)expression.CompiledExpression.DynamicInvoke(null);
			Assert.AreEqual("Hello", content);
		}

		private XrcFileResource GetFile(string relativeFilePath)
		{
			string fullPath = TestHelper.GetFile(relativeFilePath);

			var xrcFolder = new XrcFolder(System.IO.Path.GetDirectoryName(fullPath), null);
			var xrcFile = new XrcFile(fullPath, xrcFolder);
			return new XrcFileResource(xrcFile, "~/test", "~/test", new Dictionary<string, string>());
		}
    }
}
