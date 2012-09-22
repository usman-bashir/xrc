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
			var fileConfig = TestHelper.GetFile(@"sampleWebSite2\xrc.config");

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var configParserResult = new PageParserResult();
			configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
			configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
			schemaParser.Setup(p => p.Parse(fileConfig)).Returns(configParserResult);

			var pageParserResult = new PageParserResult();
			pageParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("page")));
			schemaParser.Setup(p => p.Parse(file.File.FullPath)).Returns(pageParserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

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
