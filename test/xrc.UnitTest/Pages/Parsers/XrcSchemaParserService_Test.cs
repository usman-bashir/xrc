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
using xrc.Pages.Providers;

namespace xrc.Pages.Parsers
{
	[TestClass]
    public class XrcSchemaParserService_Test
    {
        [TestMethod]
        public void It_Should_be_possible_to_parse_page_parameters()
        {
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:parameters>
		<xrc:add key='title' value='My page title' />
		<xrc:add key='timeout' value='100' type='System.Int32' />
	</xrc:parameters>
</xrc:page>
");

            Assert.AreEqual("My page title", page.Parameters["title"].Value.ToString());
            Assert.AreEqual(typeof(string), page.Parameters["title"].Value.ValueType);

            Assert.AreEqual("100", page.Parameters["timeout"].Value.ToString());
            Assert.AreEqual(typeof(int), page.Parameters["timeout"].Value.ValueType);
		}

        [TestMethod]
        public void It_Should_be_possible_to_parse_page_multiple_parameters()
        {
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:parameters>
		<xrc:add key='p1' value='My page title' allowRequestOverride='false' />
		<xrc:add key='p2' allowRequestOverride='true' />
		<xrc:add key='p3' />
		<xrc:add key='p4' />
		<xrc:add key='p5' value='p5 value' />
	</xrc:parameters>
</xrc:page>");

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
        public void It_Should_be_possible_to_parse_page_script()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
		<xrc:TestView>
			<scriptProperty>@DummyScript</scriptProperty>
		</xrc:TestView>
	</xrc:action>
</xrc:page>
");

			PageAction action = page.Actions["get"];
			var view = action.Views.Single();
			Assert.AreEqual(1, view.Properties.Count());
			Assert.AreEqual(typeof(TestView), view.Component.Type);
			Assert.AreEqual("DummyScript", view.Properties["scriptProperty"].Value.ToString());
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_action_without_method_default_to_GET()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
	</xrc:action>
</xrc:page>
");

			Assert.AreEqual(1, page.Actions.Count);
			Assert.AreEqual("get", page.Actions.Single().Method);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_action_with_explicit_get()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action method='GET'>
	</xrc:action>
</xrc:page>
");

			Assert.AreEqual(1, page.Actions.Count);
			Assert.AreEqual("get", page.Actions.Single().Method);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_actions_with_multiple_methods()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action method='GET'>
	</xrc:action>
	<xrc:action method='put'>
	</xrc:action>
	<xrc:action method='POST'>
	</xrc:action>
	<xrc:action method='DELETE'>
	</xrc:action>
</xrc:page>
");

			Assert.AreEqual(4, page.Actions.Count);
			Assert.IsNotNull(page.Actions["GET"]);
			Assert.IsNotNull(page.Actions["PUT"]);
			Assert.IsNotNull(page.Actions["POST"]);
			Assert.IsNotNull(page.Actions["DELETE"]);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_page_with_TestView_and_inline_xml_data()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
		<xrc:TestView>
			<XDocProperty xmlns=''>
				<bookstore />
			</XDocProperty>
		</xrc:TestView>
	</xrc:action>
</xrc:page>
");

            PageAction action = page.Actions["GET"];
            var view = action.Views.Single();
            Assert.AreEqual(typeof(TestView), view.Component.Type);
            XDocument xmlData = (XDocument)view.Properties["XDocProperty"].Value.Value;

			Assert.AreEqual("bookstore", xmlData.Root.Name);
        }

		[TestMethod]
		public void It_Should_be_possible_to_parse_fileInclude_page_xml()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
		<xrc:TestView>
			<XDocPropertyFile>fileInclude.xml</XDocPropertyFile>
		</xrc:TestView>
	</xrc:action>
</xrc:page>
");

			PageAction action = page.Actions["GET"];
			var view = action.Views.Single();
			Assert.AreEqual(typeof(TestView), view.Component.Type);
			Assert.AreEqual("fileInclude.xml", view.Properties["XDocPropertyFile"].Value.Value);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_fileInclude_page_text()
		{
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
		<xrc:TestView>
			<TextPropertyFile>fileInclude.txt</TextPropertyFile>
		</xrc:TestView>
	</xrc:action>
</xrc:page>
");

			PageAction action = page.Actions["GET"];
			var view = action.Views.Single();
			Assert.AreEqual(typeof(TestView), view.Component.Type);

			Assert.AreEqual("fileInclude.txt", view.Properties["TextPropertyFile"].Value.Value);
		}

        [TestMethod]
        public void It_Should_be_possible_to_parse_page_with_multiple_slots()
        {
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action method='GET'>
		<xrc:TestView slot='SLOT1' />
		<xrc:TestView slot='slot2' />
		<xrc:TestView slot='SLOT3' />
	</xrc:action>
</xrc:page>
");

            PageAction action = page.Actions["GET"];
            Assert.AreEqual(3, action.Views.Count());
            Assert.AreEqual("slot1", action.Views["SLOT1"].Slot);
            Assert.AreEqual("slot2", action.Views["slot2"].Slot);
            Assert.AreEqual("slot3", action.Views["slot3"].Slot);
        }

		[TestMethod]
        public void It_Should_be_possible_to_define_catch_exception()
        {
			var page = TargetParse(@"<?xml version='1.0' encoding='utf-8' ?>
<xrc:page xmlns:xrc='urn:xrc'>
	<xrc:action>
		<xrc:catchException url='_friendlyError' />
	</xrc:action>
</xrc:page>
");

            PageAction action = page.Actions["GET"];
			Assert.AreEqual("_friendlyError", action.CatchException.Url);
        }

        class TestView : IView
        {
            public static ComponentDefinition Definition = new ComponentDefinition("TestView", typeof(TestView));

            public XDocument XDocProperty { get; set; }
			public string XDocPropertyFile { get; set; }
			public string TextProperty { get; set; }
			public string TextPropertyFile { get; set; }
			public string ScriptProperty { get; set; }

            public void Execute(IContext context)
            {
                throw new NotImplementedException();
            }
        }

		PageDefinition TargetParse(string xrcContent)
		{
			var expectedContent = XDocument.Parse(xrcContent);

			var pageProvider = new Mock<IResourceProviderService>();
            pageProvider.Setup(p => p.ResourceToXml("~/item.xrc")).Returns(expectedContent);

			var target = new XrcSchemaParserService(pageProvider.Object,
										new Mocks.PageScriptServiceMock(),
										new Mocks.ModuleCatalogServiceMock(),
										new Mocks.ViewCatalogServiceMock(TestView.Definition));

            return target.Parse("~/item.xrc");
		}
	}
}
