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
	public class XHtmlParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_example8()
		{
			var file = GetFile(@"sampleWebSite2\example8.xrc.xhtml");

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var scriptService = new Mocks.PageScriptServiceMock();
			var moduleCatalog = new Mocks.ModuleCatalogServiceMock(new ComponentDefinition(typeof(FileModule).Name, typeof(FileModule)));
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(typeof(XHtmlView).Name, typeof(XHtmlView)));

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog, moduleCatalog, scriptService);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(typeof(XHtmlView), view.Component.Type);
			Assert.AreEqual(typeof(FileModule).Name, page.Modules.Single().Name);

			var expression = string.Format("{0}.Xml(\"{1}\")", typeof(FileModule).Name, file.File.FileName);
			Assert.AreEqual(expression, view.Properties["Content"].Value.Expression.ToString());
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
