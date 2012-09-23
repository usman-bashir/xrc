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
	public class XsltParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_xslt_with_xml()
		{
			var file = GetFile(@"sampleWebSite2\conventions_xslt.xrc.xslt");
			var viewType = typeof(XsltView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new XsltParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			Assert.AreEqual(typeof(XDocument), view.Properties["Xslt"].Value.Expression.ReturnType);
			ScriptExpression expression = (ScriptExpression)view.Properties["Xslt"].Value.Expression;
			var content = (XDocument)expression.CompiledExpression.DynamicInvoke(null);
			Assert.AreEqual("stylesheet", content.Root.Name.LocalName);

			Assert.AreEqual(typeof(XDocument), view.Properties["Data"].Value.Expression.ReturnType);
			expression = (ScriptExpression)view.Properties["Data"].Value.Expression;
			content = (XDocument)expression.CompiledExpression.DynamicInvoke(null);
			Assert.AreEqual("bookstore", content.Root.Name.LocalName);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_xslt_without_xml()
		{
			var file = GetFile(@"sampleWebSite2\conventions_xsltonly.xrc.xslt");
			var viewType = typeof(XsltView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new XsltParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			Assert.AreEqual(1, view.Properties.Count);

			Assert.AreEqual(typeof(XDocument), view.Properties["Xslt"].Value.Expression.ReturnType);
			ScriptExpression expression = (ScriptExpression)view.Properties["Xslt"].Value.Expression;
			var content = (XDocument)expression.CompiledExpression.DynamicInvoke(null);
			Assert.AreEqual("stylesheet", content.Root.Name.LocalName);
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
