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
        private Module[] _modules;

        [TestInitialize]
        public void Init()
        {
            _modules = new Module[] {new Module("File", typeof(Modules.IFileModule))};
        }

		[TestMethod]
        public void It_Should_be_possible_to_parse_example1_page_with_xslt_configured_with_data_and_xslt_using_functions()
		{
			string file = TestHelper.GetFile(@"SiteManager\example1.xrc");

            MashupParserService target = new MashupParserService(new ScriptService());

            MashupPage page = target.Parse(file, _modules);
			Assert.AreEqual(1, page.Count);
            Assert.AreEqual("My page title", page.PageParameters["title"]);
            MashupAction action = page["get"];
			var renderer = action.Single();
			Assert.AreEqual(2, renderer.Count());
            Assert.AreEqual(typeof(XsltRenderer), renderer.RendererType);
            Assert.AreEqual("File.Xml(\"books.xslt\")", renderer["Xslt"].Expression.ToString());
            Assert.AreEqual("File.Xml(\"books.xml\")", renderer["Data"].Expression.ToString());
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_example2_page_with_inline_xml_data()
		{
            string file = TestHelper.GetFile(@"SiteManager\example2.xrc");

            MashupParserService target = new MashupParserService(new ScriptService());

            MashupPage page = target.Parse(file, _modules);
            Assert.AreEqual(1, page.Count);
            Assert.AreEqual(5, page.PageParameters.Count);
            MashupAction action = page["GET"];
            var renderer = action.Single();
			Assert.AreEqual(2, renderer.Count());
			Assert.AreEqual(typeof(XsltRenderer), renderer.RendererType);
			Assert.AreEqual("File.Xml(\"books.xslt\")", renderer["Xslt"].Expression.ToString());
			XDocument xmlData = (XDocument)renderer["Data"].Value;

            var xpath = xmlData.CreateNavigator().Select("book[1]/title");
            while (xpath.MoveNext())
                Assert.AreEqual("Book 1", xpath.Current.Value);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example3_page_with_multiple_slots()
        {
            string file = TestHelper.GetFile(@"SiteManager\example3.xrc");

            MashupParserService target = new MashupParserService(new ScriptService());

            MashupPage page = target.Parse(file, _modules);
            MashupAction action = page["GET"];
            Assert.AreEqual(3, action.Count());
            Assert.AreEqual("slot1", action["SLOT1"].Slot);
            Assert.AreEqual("slot2", action["slot2"].Slot);
            Assert.AreEqual("slot3", action["slot3"].Slot);
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_example4_page_with_multiple_actions()
        {
            string file = TestHelper.GetFile(@"SiteManager\example4.xrc");

            MashupParserService target = new MashupParserService(new ScriptService());

            MashupPage page = target.Parse(file, _modules);
            Assert.AreEqual(3, page.Count);
            Assert.AreEqual("get", page["GET"].Method);
            Assert.AreEqual("delete", page["delete"].Method);
            Assert.AreEqual("post", page["Post"].Method);
        }
    }
}
