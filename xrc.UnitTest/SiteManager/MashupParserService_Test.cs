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
        public void It_Should_be_possible_to_parse_example5_page_parameter()
        {
            string file = TestHelper.GetFile(@"SiteManager\example5.xrc");

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(), 
                                        new Mocks.ModuleCatalogServiceMock(null), 
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            Assert.AreEqual("My page title", page.Parameters["title"].Value.ToString());
            Assert.AreEqual(typeof(string), page.Parameters["title"].Value.ValueType);

            Assert.AreEqual("100", page.Parameters["timeout"].Value.ToString());
            Assert.AreEqual(typeof(int), page.Parameters["timeout"].Value.ValueType);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example2_page_multiple_parameters()
        {
            string file = TestHelper.GetFile(@"SiteManager\example2.xrc");

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
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
			string file = TestHelper.GetFile(@"SiteManager\example1.xrc");

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            MashupAction action = page.Actions["get"];
			var renderer = action.Renderers.Single();
            Assert.AreEqual(1, renderer.Properties.Count());
            Assert.AreEqual(typeof(TestRenderer), renderer.Component.Type);
            Assert.AreEqual("DummyScript", renderer.Properties["scriptProperty"].Value.ToString());
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_example2_page_with_inline_xml_data()
		{
            string file = TestHelper.GetFile(@"SiteManager\example2.xrc");

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(),
                                        new Mocks.ModuleCatalogServiceMock(null),
                                        new Mocks.RendererCatalogServiceMock(TestRenderer.Definition));

            MashupPage page = target.Parse(file);
            MashupAction action = page.Actions["GET"];
            var renderer = action.Renderers.Single();
            Assert.AreEqual(typeof(TestRenderer), renderer.Component.Type);
            XDocument xmlData = (XDocument)renderer.Properties["XDocProperty"].Value.Value;

            var xpath = xmlData.CreateNavigator().Select("book[1]/title");
            while (xpath.MoveNext())
                Assert.AreEqual("Book 1", xpath.Current.Value);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example3_page_with_multiple_slots()
        {
            string file = TestHelper.GetFile(@"SiteManager\example3.xrc");

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(),
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

            MashupParserService target = new MashupParserService(new Mocks.MashupScriptServiceMock(),
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
