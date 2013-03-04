using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace xrc.Pages.TreeStructure
{
	[TestClass]
	public class XrcItem_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

		[TestMethod]
		public void It_Should_be_possible_to_get_layoutfile()
		{
			var structure = new TestPageStructure1();
			var root = structure.GetRoot();

			var index = root.Files["index.xrc"];
            Assert.AreEqual("~/_layout.xrc", index.DefaultLayoutFile.ResourceLocation);

			var newsIndex = root.Directories["news"].Files["index.xrc"];
            Assert.AreEqual("~/_layout.xrc", newsIndex.DefaultLayoutFile.ResourceLocation);

            var teamsTeamPlayerStats = root.Directories["teams"].Directories["{teamid}"].Directories["{playerid}"].Files["stats.xrc"];
            Assert.AreEqual("~/teams/{teamid}/{playerid}/_layout.xrc", teamsTeamPlayerStats.DefaultLayoutFile.ResourceLocation);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_configfile()
		{
			var structure = new TestPageStructure1();
			var root = structure.GetRoot();

			Assert.AreEqual("~/xrc.config", root.ConfigFile.ResourceLocation);

			var news = root.Directories["news"];
			Assert.IsNull(news.ConfigFile);
		}

		[TestMethod]
		public void It_Should_be_possible_to_check_IsIndex()
		{
			var structure = new TestPageStructure1();
			var root = structure.GetRoot();

			var index = root.Files["index.xrc"];
			Assert.AreEqual(true, index.IsIndex);

            var teamsTeamPlayerStats = root.Directories["teams"].Directories["{teamid}"].Directories["{playerid}"].Files["stats.xrc"];
			Assert.AreEqual(false, teamsTeamPlayerStats.IsIndex);
		}

		[TestMethod]
		public void It_Should_be_possible_to_check_IsSlot()
		{
			var structure = new TestPageStructure1();
			var root = structure.GetRoot();

            var index = root.Files["index.xrc"];
			Assert.AreEqual(false, index.IsSlot);

            var newsSlot1 = root.Directories["news"].Files["_slot1.xrc"];
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
            Assert.AreEqual("folder1", new PageDirectory(@"folder1").Name);
            Assert.AreEqual("folder1", new PageDirectory(@"FOLDER1").Name);
            Assert.AreEqual("folder1}", new PageDirectory(@"folder1}").Name);
            Assert.AreEqual("{folder1", new PageDirectory(@"{folder1").Name);
            Assert.AreEqual("{}", new PageDirectory(@"{}").Name);
            Assert.AreEqual("{test}", new PageDirectory(@"{test}").Name);
            Assert.AreEqual("{test.test}", new PageDirectory(@"{test.test}").Name);
            Assert.AreEqual("{test.test}test", new PageDirectory(@"{test.test}test").Name);
		}

		[TestMethod]
		public void It_Should_be_possible_to_check_priority_of_two_item()
		{
			var a = new PageFile("a.xrc");
            var b = new PageFile("b.xrc");
			Assert.IsTrue(a.Priority == b.Priority);

            a = new PageFile("test.xrc");
            b = new PageFile("test.xrc");
			Assert.IsTrue(a.Priority == b.Priority);

			// when an item have a dynamic paramater is less priority than a fixed item
            a = new PageFile("{p}.xrc");
            b = new PageFile("b.xrc");
			Assert.IsTrue(a.Priority > b.Priority);

			// when an item have a dynamic parameter the priority s calculated using the number of fixed characters
            a = new PageFile("lnooooongerprefix{p}.xrc");
            b = new PageFile("shorterprefix{p}.xrc");
			Assert.IsTrue(a.Priority < b.Priority);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_name()
		{
            Assert.AreEqual("test", new PageFile("test.xrc").Name);
            Assert.AreEqual("test", new PageFile("test.xrc.xml").Name);
            Assert.AreEqual("test.xml", new PageFile("test.xml.xrc").Name);
            Assert.AreEqual("{param}", new PageFile("{param}.xrc").Name);
            Assert.AreEqual("{pageurl...}.ext", new PageFile("{pageUrl...}.ext.xrc").Name);
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_url()
		{
            Assert.AreEqual("~/test", new PageFile("test.xrc").BuildUrl().ToString());
            Assert.AreEqual("~/test", new PageFile("test.xrc").BuildUrl(new UriSegmentParameterList { { "test", "p1" } }).ToString());
            Assert.AreEqual("~/{test}", new PageFile("{test}.xrc").BuildUrl().ToString());
            Assert.AreEqual("~/p1", new PageFile("{test}.xrc").BuildUrl(new UriSegmentParameterList { { "test", "p1" } }).ToString());
            Assert.AreEqual("~/{test}", new PageFile("{test}.xrc").BuildUrl(new UriSegmentParameterList { { "test2", "p1" } }).ToString());
            Assert.AreEqual("~/prefix.p1.suffix", new PageFile("prefix.{test}.suffix.xrc").BuildUrl(new UriSegmentParameterList { { "test", "p1" } }).ToString());
		}

		[TestMethod]
		public void It_Should_not_be_possible_to_have_2_items_with_the_same_name()
		{
			TestHelper.Throws<DuplicateItemException>(() => new PageDirectory("~",
                                                new PageFile("test.xrc"),
                                                new PageFile("Test.xrc")));

            TestHelper.Throws<DuplicateItemException>(() => new PageDirectory("~",
                                                new PageDirectory("test"),
                                                new PageDirectory("Test")));
		}
	}
}
