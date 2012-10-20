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
using xrc.Pages.Providers.Common;

namespace xrc.Pages.Providers.FileSystem
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
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xml"));
			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xslt"));
			Assert.AreEqual("test.html", XrcItem.GetFileLogicalName(@"test.html.xrc.xslt"));

			Assert.AreEqual("test", XrcItem.GetFileLogicalName(@"test.xrc.xs_lt"));
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_file_extension()
		{
			Assert.AreEqual(".xrc", XrcItem.GetFileExtension(@"test.xrc"));
			Assert.AreEqual(".xml", XrcItem.GetFileExtension(@"test.xrc.xml"));
			Assert.AreEqual(".xslt", XrcItem.GetFileExtension(@"test.xrc.xslt"));
			Assert.AreEqual(".xslt", XrcItem.GetFileExtension(@"test.html.xrc.xslt"));
			Assert.AreEqual(".xslt", XrcItem.GetFileExtension(@"c:\folder1\test.html.xrc.xslt"));
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_directory_parameter()
		{
			Assert.AreEqual("folder1", XrcItem.GetDirectoryLogicalName(@"folder1"));
			Assert.AreEqual("folder1}", XrcItem.GetDirectoryLogicalName(@"folder1}"));
			Assert.AreEqual("{folder1", XrcItem.GetDirectoryLogicalName(@"{folder1"));
			Assert.AreEqual("{}", XrcItem.GetDirectoryLogicalName(@"{}"));
			Assert.AreEqual("{test}", XrcItem.GetDirectoryLogicalName(@"{test}"));
			Assert.AreEqual("{test.test}", XrcItem.GetDirectoryLogicalName(@"{test.test}"));
			Assert.AreEqual("{test.test}test", XrcItem.GetDirectoryLogicalName(@"{test.test}test"));
		}
    }
}
