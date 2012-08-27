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

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
    public class PageParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example5_page_parameter()
        {
			string file = TestHelper.GetFile(@"Pages\example5.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(), 
                                        new Mocks.ModuleCatalogServiceMock(null), 
                                        new Mocks.ViewCatalogServiceMock(TestView.Definition));

            PageParserResult page = target.Parse(file);
            Assert.AreEqual("My page title", page.Parameters["title"].Value.ToString());
            Assert.AreEqual(typeof(string), page.Parameters["title"].Value.ValueType);

            Assert.AreEqual("100", page.Parameters["timeout"].Value.ToString());
            Assert.AreEqual(typeof(int), page.Parameters["timeout"].Value.ValueType);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example2_page_multiple_parameters()
        {
			string file = TestHelper.GetFile(@"Pages\example2.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
            Assert.AreEqual(5, page.Parameters.Count);
            Assert.AreEqual(false, page.Parameters["p1"].AllowRequestOverride);
            Assert.AreEqual("My page title", page.Parameters["p1"].Value.Value);
            Assert.AreEqual(true, page.Parameters["p2"].AllowRequestOverride);
            Assert.AreEqual(true, page.Parameters["p2"].Value.IsEmpty());
            Assert.IsNull(page.Parameters["p2"].Value.Value);
            Assert.IsNull(page.Parameters["p2"].Value.Expression);
            Assert.AreEqual(false, page.Parameters["p3"].AllowRequestOverride);
        }

		[TestMethod]
        public void It_Should_be_possible_to_parse_example1_page_using_script()
		{
			string file = TestHelper.GetFile(@"Pages\example1.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
										new Mocks.ModuleCatalogServiceMock(null),
										new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
			PageAction action = page.Actions["get"];
			var view = action.Views.Single();
			Assert.AreEqual(1, view.Properties.Count());
			Assert.AreEqual(typeof(TestView), view.Component.Type);
			Assert.AreEqual("DummyScript", view.Properties["scriptProperty"].Value.ToString());
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_example6_action_without_method_default_to_GET()
		{
			string file = TestHelper.GetFile(@"Pages\example6.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
										new Mocks.ModuleCatalogServiceMock(null),
										new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
			Assert.AreEqual(1, page.Actions.Count);
			Assert.AreEqual("get", page.Actions.First().Method);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_example2_page_with_inline_xml_data()
		{
            string file = TestHelper.GetFile(@"Pages\example2.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
            PageAction action = page.Actions["GET"];
            var view = action.Views.Single();
            Assert.AreEqual(typeof(TestView), view.Component.Type);
            XDocument xmlData = (XDocument)view.Properties["XDocProperty"].Value.Value;

            var xpath = xmlData.CreateNavigator().Select("book[1]/title");
            while (xpath.MoveNext())
                Assert.AreEqual("Book 1", xpath.Current.Value);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example3_page_with_multiple_slots()
        {
			string file = TestHelper.GetFile(@"Pages\example3.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
            PageAction action = page.Actions["GET"];
            Assert.AreEqual(3, action.Views.Count());
            Assert.AreEqual("slot1", action.Views["SLOT1"].Slot);
            Assert.AreEqual("slot2", action.Views["slot2"].Slot);
            Assert.AreEqual("slot3", action.Views["slot3"].Slot);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example4_page_with_multiple_actions()
        {
			string file = TestHelper.GetFile(@"Pages\example4.xrc");

			PageParserService target = new PageParserService(new Mocks.PageScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.ViewCatalogServiceMock(TestView.Definition));

			PageParserResult page = target.Parse(file);
            Assert.AreEqual(3, page.Actions.Count);
            Assert.AreEqual("get", page.Actions["GET"].Method);
            Assert.AreEqual("delete", page.Actions["delete"].Method);
            Assert.AreEqual("post", page.Actions["Post"].Method);
        }

        class TestView : IView
        {
            public static ComponentDefinition Definition = new ComponentDefinition("TestView", typeof(TestView));

            public TestObject scriptProperty { get; set; }
            public XDocument XDocProperty { get; set; }
            public string PrimitiveProperty { get; set; }

            public void Execute(IContext context)
            {
                throw new NotImplementedException();
            }
        }

        class TestObject
        {
        }
    }
}
