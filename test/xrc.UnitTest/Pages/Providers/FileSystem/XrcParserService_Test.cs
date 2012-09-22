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

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
	public class XrcParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_example7_folder_parameters()
		{
			var file = GetFile(@"sampleWebSite2\example7.xrc");

			// TODO Da rivedere, fatto così non è un vero unit test

			var schemaParser = new XrcSchemaParserService(new Mocks.PageScriptServiceMock(),
										new Mocks.ModuleCatalogServiceMock(null),
										new Mocks.ViewCatalogServiceMock(null));

			var target = new XrcParserService(schemaParser, schemaParser);

			PageParserResult page = target.Parse(file);
			Assert.AreEqual("folder", page.Parameters["folderParameter1"].Value.ToString());
			Assert.AreEqual("page", page.Parameters["folderParameter2"].Value.ToString());
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
