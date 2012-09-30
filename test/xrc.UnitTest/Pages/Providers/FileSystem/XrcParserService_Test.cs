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
using System.IO;

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
			var fileConfig = TestHelper.GetPath(@"sampleWebSite2\xrc.config");

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var configParserResult = new PageParserResult();
			configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
			configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
			schemaParser.Setup(p => p.Parse(fileConfig.ToLowerInvariant())).Returns(configParserResult);

			var pageParserResult = new PageParserResult();
			pageParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("page")));
			schemaParser.Setup(p => p.Parse(file.File.FullPath.ToLowerInvariant())).Returns(pageParserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(file);
			Assert.AreEqual("folder", page.Parameters["folderParameter1"].Value.ToString());
			Assert.AreEqual("page", page.Parameters["folderParameter2"].Value.ToString());
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_example1_page_and_check_layout()
		{
			var file = GetFile(@"sampleWebSite2\example1.xrc");
			var fileConfig = TestHelper.GetPath(@"sampleWebSite2\xrc.config");

			var schemaParser = new Mock<IXrcSchemaParserService>();

			var configParserResult = new PageParserResult();
			configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
			configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
			schemaParser.Setup(p => p.Parse(fileConfig.ToLowerInvariant())).Returns(configParserResult);

			var parserResult = new PageParserResult();
			parserResult.Actions.Add(new PageAction("GET"));
			schemaParser.Setup(p => p.Parse(file.File.FullPath.ToLowerInvariant())).Returns(parserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(file);
			PageAction action = page.Actions["get"];
			Assert.AreEqual("~/_layout", action.Layout);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_example1_slot_and_check_layout()
		{
			var file = GetFile(@"sampleWebSite2\_slot1.xrc");
			var fileConfig = TestHelper.GetPath(@"sampleWebSite2\xrc.config");

			var schemaParser = new Mock<IXrcSchemaParserService>();

			var configParserResult = new PageParserResult();
			configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
			configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
			schemaParser.Setup(p => p.Parse(fileConfig.ToLowerInvariant())).Returns(configParserResult);

			var parserResult = new PageParserResult();
			parserResult.Actions.Add(new PageAction("GET"));
			schemaParser.Setup(p => p.Parse(file.File.FullPath.ToLowerInvariant())).Returns(parserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(file);
			PageAction action = page.Actions["get"];
			Assert.IsNull(action.Layout);
		}

		private XrcFileResource GetFile(string relativeFilePath)
		{
			string fullPath = TestHelper.GetPath(relativeFilePath);

			var rootConfig = new Mocks.RootPathConfigMock("~/test", Path.GetDirectoryName(fullPath));
			var xrcFolder = new XrcFolder(rootConfig);
			var xrcFile = new XrcFile(xrcFolder, Path.GetFileName(fullPath));
			return new XrcFileResource(xrcFile, "~/test", new Dictionary<string, string>());
		}
    }
}
