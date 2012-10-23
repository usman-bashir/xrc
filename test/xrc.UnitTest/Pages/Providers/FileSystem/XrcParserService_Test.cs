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
using System.IO;
using xrc.Pages.Providers.Common.Parsers;
using xrc.Pages.Providers.Common;

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
		public void It_Should_be_possible_to_parse_folder_parameters()
		{
			var item = XrcItem.NewXrcFile("item.xrc");
			var config = XrcItem.NewConfigFile();
			var xrcRoot = XrcItem.NewRoot("~", item, config);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var configParserResult = new PageParserResult();
			configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
			configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
			schemaParser.Setup(p => p.Parse(config)).Returns(configParserResult);

			var pageParserResult = new PageParserResult();
			pageParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("page")));
			schemaParser.Setup(p => p.Parse(item)).Returns(pageParserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(item);
			Assert.AreEqual("folder", page.Parameters["folderParameter1"].Value.ToString());
			Assert.AreEqual("page", page.Parameters["folderParameter2"].Value.ToString());
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_page_and_check_layout()
		{
			var item = XrcItem.NewXrcFile("item.xrc");
			var layout = XrcItem.NewXrcFile("_layout.xrc");
			var xrcRoot = XrcItem.NewRoot("~", item, layout);

			var schemaParser = new Mock<IXrcSchemaParserService>();

			var parserResult = new PageParserResult();
			parserResult.Actions.Add(new PageAction("GET"));
			schemaParser.Setup(p => p.Parse(item)).Returns(parserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(item);
			PageAction action = page.Actions["get"];
			Assert.AreEqual("~/_layout", action.Layout);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_page_with_layout_and_path_parameter()
		{
			var item = XrcItem.NewXrcFile("item.xrc");
			var layout = XrcItem.NewXrcFile("_layout.xrc");
			var xrcPathParam = XrcItem.NewDirectory("{param}", item, layout);
			var xrcRoot = XrcItem.NewRoot("~", xrcPathParam);

			var schemaParser = new Mock<IXrcSchemaParserService>();

			var parserResult = new PageParserResult();
			parserResult.Actions.Add(new PageAction("GET"));
			schemaParser.Setup(p => p.Parse(item)).Returns(parserResult);

			var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

			PageParserResult page = target.Parse(item);
			PageAction action = page.Actions["get"];
			Assert.AreEqual("~/{param}/_layout", action.Layout);
		}

		//[TestMethod]
		//public void It_Should_be_possible_to_parse_example1_slot_and_check_layout()
		//{
		//    var file = GetFile(@"sampleWebSite2\_slot1.xrc");
		//    var fileConfig = TestHelper.GetPath(@"sampleWebSite2\xrc.config");

		//    var schemaParser = new Mock<IXrcSchemaParserService>();

		//    var configParserResult = new PageParserResult();
		//    configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
		//    configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));
		//    schemaParser.Setup(p => p.Parse(fileConfig.ToLowerInvariant())).Returns(configParserResult);

		//    var parserResult = new PageParserResult();
		//    parserResult.Actions.Add(new PageAction("GET"));
		//    schemaParser.Setup(p => p.Parse(file.File.FullPath.ToLowerInvariant())).Returns(parserResult);

		//    var target = new XrcParserService(schemaParser.Object, schemaParser.Object);

		//    PageParserResult page = target.Parse(file);
		//    PageAction action = page.Actions["get"];
		//    Assert.IsNull(action.Layout);
		//}
    }
}
