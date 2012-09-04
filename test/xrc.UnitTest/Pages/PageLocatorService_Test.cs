using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using xrc.Configuration;
using Moq;

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
	public class PageLocatorService_Test
	{
		public PageLocatorService_Test()
		{
		}

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
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }

		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion


		[TestMethod]
        public void It_should_be_possible_to_Locate_File()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSiteStructure", TestHelper.GetFile("sampleWebSiteStructure"));
			PageLocatorService target = new PageLocatorService(workingPath);
            var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			// Base functionalities
            Assert.AreEqual(target.Locate("/").FullPath, Path.Combine(appPath, "index.xrc"));
            Assert.AreEqual(target.Locate("/").CanonicalVirtualUrl, "~/");
			Assert.AreEqual(target.Locate("/").VirtualPath, "~/sampleWebSiteStructure/");
			Assert.AreEqual(target.Locate("/about").FullPath, Path.Combine(appPath, "about.xrc"));
			Assert.AreEqual(target.Locate("/about").CanonicalVirtualUrl, "~/about");
			Assert.AreEqual(target.Locate("/about").VirtualPath, "~/sampleWebSiteStructure/");
			Assert.AreEqual(target.Locate("/index").CanonicalVirtualUrl, "~/");
            Assert.AreEqual(target.Locate("/athletes").FullPath, Path.Combine(appPath, @"athletes\index.xrc"));
			Assert.AreEqual(target.Locate("/athletes").CanonicalVirtualUrl, "~/athletes/");
			Assert.AreEqual(target.Locate("/athletes").VirtualPath, "~/sampleWebSiteStructure/athletes/");
			Assert.AreEqual(target.Locate("/ATHLETES").CanonicalVirtualUrl, "~/athletes/");
			Assert.AreEqual(target.Locate("/ATHLETES/indeX").CanonicalVirtualUrl, "~/athletes/");
            Assert.AreEqual(target.Locate("").FullPath, Path.Combine(appPath, "index.xrc"));
            Assert.AreEqual(target.Locate("athletes").FullPath, Path.Combine(appPath, @"athletes\index.xrc"));

			// Dynamic pages
            Assert.AreEqual(target.Locate("/athletes/totti").FullPath, Path.Combine(appPath, @"athletes\{athleteid}\index.xrc"));
            Assert.AreEqual(target.Locate("/athletes/totti").UrlSegmentsParameters["athleteid"], "totti");
            Assert.AreEqual(target.Locate("/athletes/ToTTi").UrlSegmentsParameters["athleteid"], "totti");
            Assert.AreEqual(target.Locate("/teams/torino").FullPath, Path.Combine(appPath, @"teams\{teamid}\index.xrc"));
			Assert.AreEqual(target.Locate("/teams/torino").CanonicalVirtualUrl, "~/teams/torino/");
			Assert.AreEqual(target.Locate("/teams/torino").VirtualPath, "~/sampleWebSiteStructure/teams/{teamid}/");
			Assert.AreEqual(target.Locate("/teams/verona").VirtualPath, "~/sampleWebSiteStructure/teams/{teamid}/");
			Assert.AreEqual(target.Locate("/teams/torino/matches").FullPath, Path.Combine(appPath, @"teams\{teamid}\matches.xrc"));
			Assert.AreEqual(target.Locate("/TEAMS/TORINO/MATCHES").CanonicalVirtualUrl, "~/teams/torino/matches");
            Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["teamid"], "torino");
            Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["playerid"], "cravero");
            Assert.AreEqual(target.Locate("/teams/torino/matches.xrc").UrlSegmentsParameters["playerid"], "matches.xrc");

			// Default page index and etensions
            Assert.AreEqual(target.Locate("/athletes/totti/index").FullPath, Path.Combine(appPath, @"athletes\{athleteid}\index.xrc"));

			// folder config file
			Assert.AreEqual(target.Locate("/athletes").Parent.GetConfigFile(), Path.Combine(appPath, @"athletes\xrcFolder.config"));
			Assert.AreEqual(target.Locate("/teams").Parent.GetConfigFile(), null);

			// File not found == null
            Assert.IsNull(target.Locate("notvalid"));
            Assert.IsNull(target.Locate("/notvalid"));
            Assert.IsNull(target.Locate("/athletes/totti/notfound"));
            Assert.IsNull(target.Locate("/athletes/totti/index.xrc"));

            // Absolute uri not supported
            TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/absoluteuri"));
            TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/"));
            TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1.com"));
        }
	}
}
