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
	public class XrcFileSystemHelper_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_to_get_file_name()
        {
			Assert.AreEqual("test", XrcFileSystemHelper.GetFileName(@"c:\folder1\test.xrc"));
			Assert.AreEqual("test", XrcFileSystemHelper.GetFileName(@"c:\folder1\test.xrc.xml"));
			Assert.AreEqual("test", XrcFileSystemHelper.GetFileName(@"c:\folder1\test.xrc.xslt"));
			Assert.AreEqual("test.html", XrcFileSystemHelper.GetFileName(@"c:\folder1\test.html.xrc.xslt"));

			Assert.AreEqual("test", XrcFileSystemHelper.GetFileName(@"c:\folder1\test.xrc.xs_lt"));
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_file_extension()
		{
			Assert.AreEqual(".xrc", XrcFileSystemHelper.GetFileExtension(@"c:\folder1\test.xrc"));
			Assert.AreEqual(".xml", XrcFileSystemHelper.GetFileExtension(@"c:\folder1\test.xrc.xml"));
			Assert.AreEqual(".xslt", XrcFileSystemHelper.GetFileExtension(@"c:\folder1\test.xrc.xslt"));
			Assert.AreEqual(".xslt", XrcFileSystemHelper.GetFileExtension(@"c:\folder1\test.html.xrc.xslt"));
		}

		[TestMethod]
		public void It_Should_be_possible_to_get_directory_parameter()
		{
			Assert.IsNull(XrcFileSystemHelper.GetDirectoryParameterName(@"folder1"));
			Assert.IsNull(XrcFileSystemHelper.GetDirectoryParameterName(@"folder1}"));
			Assert.IsNull(XrcFileSystemHelper.GetDirectoryParameterName(@"{folder1"));
			Assert.IsNull(XrcFileSystemHelper.GetDirectoryParameterName(@"{}"));
			Assert.AreEqual("test", XrcFileSystemHelper.GetDirectoryParameterName(@"{test}"));
			Assert.AreEqual("test.test", XrcFileSystemHelper.GetDirectoryParameterName(@"{test.test}"));
		}
    }
}
