﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace xrc.Sites
{
   
    /// <summary>
    ///This is a test class for SiteConfiguration_Test and is intended
    ///to contain all SiteConfiguration_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class SiteConfiguration_Test
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


		//[TestMethod()]
		//public void It_should_be_possible_to_get_the_relative_url_of_a_site()
		//{
		//    SiteConfiguration target = new SiteConfiguration("test", new Uri("http://contoso.com"), 
		//                                                        new Dictionary<string, string>(),
		//                                                        new Uri("https://contoso.com:443/"));

		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com:80/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://CONTOSO.com/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("https://contoso.com:443/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com//")));
		//    Assert.AreEqual(new Uri("test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/test.html")));
		//    Assert.AreEqual(new Uri("test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com//test.html")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com")));
		//    Assert.AreEqual(new Uri("folder/test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/folder/test.html")));
		//    Assert.AreEqual(new Uri("test.html?prova=3", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/test.html?prova=3")));
		//    Assert.AreEqual(new Uri("test.html?prova=3", UriKind.Relative), target.ToRelativeUrl(new Uri("https://contoso.com:443/test.html?prova=3")));

		//    // relative url are not supported
		//    TestHelper.Throws<UriFormatException>(() => target.ToRelativeUrl(new Uri("test.html", UriKind.Relative)));

		//    // sites not configured
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("https://contoso.com:8443/test.html?prova=3")));
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("ftp://contoso.com:8443/test.html?prova=3")));
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("http://northwind.com/test.html?prova=3")));
		//}

		//[TestMethod()]
		//public void It_should_be_possible_to_get_the_relative_url_of_a_site_with_virtual_path()
		//{
		//    SiteConfiguration target = new SiteConfiguration("test", new Uri("http://contoso.com/vpath"),
		//                                                        new Dictionary<string, string>(),
		//                                                        new Uri("https://contoso.com:443/vpath/"));

		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com:80/vpath/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("https://contoso.com:443/vpath/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://CONTOSO.com/VPATH/")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath//")));
		//    Assert.AreEqual(new Uri("test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath/test.html")));
		//    Assert.AreEqual(new Uri("test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath//test.html")));
		//    Assert.AreEqual(new Uri("", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("folder/test.html", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath/folder/test.html")));
		//    Assert.AreEqual(new Uri("test.html?prova=3", UriKind.Relative), target.ToRelativeUrl(new Uri("http://contoso.com/vpath/test.html?prova=3")));
		//    Assert.AreEqual(new Uri("test.html?prova=3", UriKind.Relative), target.ToRelativeUrl(new Uri("https://contoso.com:443/vpath/test.html?prova=3")));

		//    // relative url are not supported
		//    TestHelper.Throws<UriFormatException>(() => target.ToRelativeUrl(new Uri("vpath/test.html", UriKind.Relative)));
		//    TestHelper.Throws<UriFormatException>(() => target.ToRelativeUrl(new Uri("/vpath/test.html", UriKind.Relative)));

		//    // sites not configured
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("https://contoso.com:8443/vpath/test.html?prova=3")));
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("ftp://contoso.com:8443/vpath/test.html?prova=3")));
		//    TestHelper.Throws<SiteConfigurationMismatchException>(() => target.ToRelativeUrl(new Uri("http://northwind.com/vpath/test.html?prova=3")));
		//}

		//[TestMethod()]
		//public void It_should_be_possible_to_GetAbsoluteUrl()
		//{
		//    SiteConfiguration target = new SiteConfiguration("test", new Uri("http://contoso.com/vpath"),
		//                                                        new Dictionary<string, string>(),
		//                                                        new Uri("https://contoso.com:443/vpath/"));

		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/"), 
		//        target.ToAbsoluteUrl("~", new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("https://contoso.com/vpath/"), 
		//        target.ToAbsoluteUrl("~", new Uri("https://contoso.com:443/vpath/")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/"), 
		//        target.ToAbsoluteUrl("~", new Uri("http://contoso.com/vpath/test/page?a=1")));
		//    Assert.AreEqual(new Uri("https://contoso.com/vpath/"), 
		//        target.ToAbsoluteUrl("~", new Uri("https://contoso.com:443/vpath/test/page?a=1")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/test.html"), 
		//        target.ToAbsoluteUrl("~/test.html", new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/path/index.html?test=12"), 
		//        target.ToAbsoluteUrl("~/path/index.html?test=12", new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("http://northwind.com/test"), 
		//        target.ToAbsoluteUrl("http://northwind.com/test", new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("http://contoso.com/test.html"), 
		//        target.ToAbsoluteUrl("/test.html", new Uri("http://contoso.com/vpath")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/test.html"), 
		//        target.ToAbsoluteUrl("test.html", new Uri("http://contoso.com/vpath/")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/test.html"), 
		//        target.ToAbsoluteUrl("test.html", new Uri("http://contoso.com/vpath/index.html")));
		//    Assert.AreEqual(new Uri("http://contoso.com/vpath/test.html"), 
		//        target.ToAbsoluteUrl("test.html", new Uri("http://contoso.com/vpath/index.html?p1=test")));
		//    Assert.AreEqual(new Uri("https://contoso.com/vpath/test.html?p2=t2"), 
		//        target.ToAbsoluteUrl("test.html?p2=t2", new Uri("https://contoso.com/vpath/index.html?p1=test")));
		//    Assert.AreEqual(new Uri("http://contoso.com/test.html"), 
		//        target.ToAbsoluteUrl("/test.html", new Uri("http://contoso.com/vpath/index.html")));
		//}
    }
}
