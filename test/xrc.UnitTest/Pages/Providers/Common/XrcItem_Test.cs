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

namespace xrc.Pages.Providers.Common
{
	[TestClass]
	public class XrcItem_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_to_get_file_name()
        {
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc"));
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"TEST.XRC"));
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xml"));
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xslt"));
			Assert.AreEqual("test.html", XrcItem.GetFileLogicalName(@"test.html.xrc.xslt"));
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xs_lt"));

			TestHelper.Throws<NotSupportedException>(() => XrcItem.GetFileLogicalName(@"test.xrck")); // not valid
			TestHelper.Throws<NotSupportedException>(() => XrcItem.GetFileLogicalName(@"test.xrchk.xs_lt")); // not valid
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_layoutfile()
		{
			var structure = new TestPageStructure();
			var root = structure.GetRoot();

			var index = root.Items["index.xrc"];
			Assert.AreEqual("~/_layout.xrc", index.LayoutFile.ResourceLocation);

			var newsIndex = root.Items["news"].Items["index.xrc"];
			Assert.AreEqual("~/_layout.xrc", newsIndex.LayoutFile.ResourceLocation);

			var teamsTeamPlayerStats = root.Items["teams"].Items["{teamid}"].Items["{playerid}"].Items["stats.xrc"];
			Assert.AreEqual("~/teams/{teamid}/{playerid}/_layout.xrc", teamsTeamPlayerStats.LayoutFile.ResourceLocation);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_configfile()
		{
			var structure = new TestPageStructure();
			var root = structure.GetRoot();

			Assert.AreEqual("~/xrc.config", root.ConfigFile.ResourceLocation);

			var news = root.Items["news"];
			Assert.IsNull(news.ConfigFile);
		}

		[TestMethod]
		public void It_Should_be_possible_to_check_IsIndex()
		{
			var structure = new TestPageStructure();
			var root = structure.GetRoot();

			var index = root.Items["index.xrc"];
			Assert.AreEqual(true, index.IsIndex);

			var teamsTeamPlayerStats = root.Items["teams"].Items["{teamid}"].Items["{playerid}"].Items["stats.xrc"];
			Assert.AreEqual(false, teamsTeamPlayerStats.IsIndex);
		}

		[TestMethod]
		public void It_Should_be_possible_to_check_IsSlot()
		{
			var structure = new TestPageStructure();
			var root = structure.GetRoot();

			var index = root.Items["index.xrc"];
			Assert.AreEqual(false, index.IsSlot);

			var newsSlot1 = root.Items["news"].Items["_slot1.xrc"];
			Assert.AreEqual(true, newsSlot1.IsSlot);
		}

		//[TestMethod]
		//public void It_Should_be_possible_to_get_file_extension()
		//{
		//    Assert.AreEqual(".xrc", XrcItem.GetFileExtension(@"test.xrc"));
		//    Assert.AreEqual(".xrc", XrcItem.GetFileExtension(@"TEST.XRC"));
		//    Assert.AreEqual(".xrc.xml", XrcItem.GetFileExtension(@"test.xrc.xml"));
		//    Assert.AreEqual(".xrc.xslt", XrcItem.GetFileExtension(@"test.xrc.xslt"));
		//    Assert.AreEqual(".xrc.xslt", XrcItem.GetFileExtension(@"test.html.xrc.xslt"));
		//    Assert.AreEqual(".xrc.xslt", XrcItem.GetFileExtension(@"c:\folder1\test.html.xrc.xslt"));

		//    TestHelper.Throws<NotSupportedException>(() => XrcItem.GetFileExtension(@"test.xrck")); // not valid
		//    TestHelper.Throws<NotSupportedException>(() => XrcItem.GetFileExtension(@"test.xrchk.xs_lt")); // not valid
		//}

		[TestMethod]
		public void It_Should_be_possible_to_get_directory_name()
		{
			Assert.AreEqual("folder1", XrcItem.GetDirectoryLogicalName(@"folder1"));
			Assert.AreEqual("folder1", XrcItem.GetDirectoryLogicalName(@"FOLDER1"));
			Assert.AreEqual("folder1}", XrcItem.GetDirectoryLogicalName(@"folder1}"));
			Assert.AreEqual("{folder1", XrcItem.GetDirectoryLogicalName(@"{folder1"));
			Assert.AreEqual("{}", XrcItem.GetDirectoryLogicalName(@"{}"));
			Assert.AreEqual("{test}", XrcItem.GetDirectoryLogicalName(@"{test}"));
			Assert.AreEqual("{test.test}", XrcItem.GetDirectoryLogicalName(@"{test.test}"));
			Assert.AreEqual("{test.test}test", XrcItem.GetDirectoryLogicalName(@"{test.test}test"));
		}

		//[TestMethod]
		//public void It_Should_be_possible_to_get_config_name()
		//{
		//    Assert.AreEqual("xrc.config", XrcItem.GetConfigLogicalName(@"xrc.config"));
		//    Assert.AreEqual("xrc.config", XrcItem.GetConfigLogicalName(@"xrc.config"));

		//    TestHelper.Throws<NotSupportedException>(() => XrcItem.GetConfigLogicalName(@"test.XRC"));
		//    TestHelper.Throws<NotSupportedException>(() => XrcItem.GetConfigLogicalName(@"test.config"));
		//}

		[TestMethod]
		public void It_Should_be_possible_to_check_priority_of_two_item()
		{
			XrcItem a = XrcItem.NewXrcFile("a.xrc");
			XrcItem b = XrcItem.NewXrcFile("b.xrc");
			Assert.IsTrue(a.Priority == b.Priority);

			a = XrcItem.NewXrcFile("test.xrc");
			b = XrcItem.NewXrcFile("test.xrc");
			Assert.IsTrue(a.Priority == b.Priority);

			// when an item have a dynamic paramater is less priority than a fixed item
			a = XrcItem.NewXrcFile("{p}.xrc");
			b = XrcItem.NewXrcFile("b.xrc");
			Assert.IsTrue(a.Priority > b.Priority);

			// when an item have a dynamic parameter the priority s calculated using the number of fixed characters
			a = XrcItem.NewXrcFile("lnooooongerprefix{p}.xrc");
			b = XrcItem.NewXrcFile("shorterprefix{p}.xrc");
			Assert.IsTrue(a.Priority < b.Priority);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_name()
		{
			Assert.AreEqual("test", XrcItem.NewXrcFile("test.xrc").Name);
			Assert.AreEqual("test", XrcItem.NewXrcFile("test.xrc.xml").Name);
			Assert.AreEqual("test.xml", XrcItem.NewXrcFile("test.xml.xrc").Name);
			Assert.AreEqual("{param}", XrcItem.NewXrcFile("{param}.xrc").Name);
			Assert.AreEqual("{pageurl_catch-all}.ext", XrcItem.NewXrcFile("{pageUrl_CATCH-ALL}.ext.xrc").Name);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_url()
		{
			Assert.AreEqual("~/test", XrcItem.NewXrcFile("test.xrc").BuildUrl().ToString());
			Assert.AreEqual("~/test", XrcItem.NewXrcFile("test.xrc").BuildUrl(new Dictionary<string, string> { { "test", "p1" } }).ToString());
			Assert.AreEqual("~/{test}", XrcItem.NewXrcFile("{test}.xrc").BuildUrl().ToString());
			Assert.AreEqual("~/p1", XrcItem.NewXrcFile("{test}.xrc").BuildUrl(new Dictionary<string, string>{{"test", "p1"}}).ToString());
			Assert.AreEqual("~/{test}", XrcItem.NewXrcFile("{test}.xrc").BuildUrl(new Dictionary<string, string> { { "test2", "p1" } }).ToString());
			Assert.AreEqual("~/prefix.p1.suffix", XrcItem.NewXrcFile("prefix.{test}.suffix.xrc").BuildUrl(new Dictionary<string, string> { { "test", "p1" } }).ToString());
		}

	}
}
