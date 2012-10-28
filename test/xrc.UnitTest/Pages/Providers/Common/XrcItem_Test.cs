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
    }
}
