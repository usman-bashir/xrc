using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xrc.Renderers;
using xrc.Script;
using Moq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace xrc.SiteManager
{
	[TestClass]
    public class MashupParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example1_page_parameter()
        {
            string file = TestHelper.GetFile(@"SiteManager\example5.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(), 
                                        new Mocks.ModuleCatalogServiceMock(null), 
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            Assert.AreEqual("My page title", page.PageParameters["title"]);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example2_page_multiple_parameters()
        {
            string file = TestHelper.GetFile(@"SiteManager\example2.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            Assert.AreEqual(5, page.PageParameters.Count);
        }

		[TestMethod]
        public void It_Should_be_possible_to_parse_example1_page_using_script()
		{
			string file = TestHelper.GetFile(@"SiteManager\example1.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(true, null, "DummyScript"),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            MashupAction action = page.Actions["get"];
			var renderer = action.Renderers.Single();
            Assert.AreEqual(1, renderer.Properties.Count());
            Assert.AreEqual(typeof(TestRenderer), renderer.Component.Type);
            Assert.AreEqual("DummyScript", renderer.Properties["scriptProperty"].Expression.ToString());
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_example2_page_with_inline_xml_data()
		{
            string file = TestHelper.GetFile(@"SiteManager\example2.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            MashupAction action = page.Actions["GET"];
            var renderer = action.Renderers.Single();
            Assert.AreEqual(typeof(TestRenderer), renderer.Component.Type);
            XDocument xmlData = (XDocument)renderer.Properties["XDocProperty"].Value;

            var xpath = xmlData.CreateNavigator().Select("book[1]/title");
            while (xpath.MoveNext())
                Assert.AreEqual("Book 1", xpath.Current.Value);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example3_page_with_multiple_slots()
        {
            string file = TestHelper.GetFile(@"SiteManager\example3.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            MashupAction action = page.Actions["GET"];
            Assert.AreEqual(3, action.Renderers.Count());
            Assert.AreEqual("slot1", action.Renderers["SLOT1"].Slot);
            Assert.AreEqual("slot2", action.Renderers["slot2"].Slot);
            Assert.AreEqual("slot3", action.Renderers["slot3"].Slot);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example4_page_with_multiple_actions()
        {
            string file = TestHelper.GetFile(@"SiteManager\example4.xrc");

            MashupParserService target = new MashupParserService(new Mocks.ScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            Assert.AreEqual(3, page.Actions.Count);
            Assert.AreEqual("get", page.Actions["GET"].Method);
            Assert.AreEqual("delete", page.Actions["delete"].Method);
            Assert.AreEqual("post", page.Actions["Post"].Method);
        }

        class TestRenderer : IRenderer
        {
            public static ComponentDefinition Definition = new ComponentDefinition("TestRenderer", typeof(TestRenderer));

            public TestObject scriptProperty { get; set; }
            public XDocument XDocProperty { get; set; }
            public string PrimitiveProperty { get; set; }

            public void RenderRequest(IContext context)
            {
                throw new NotImplementedException();
            }
        }

        class TestObject
        {
        }
    }
}
