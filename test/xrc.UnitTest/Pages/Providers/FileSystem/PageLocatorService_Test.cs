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
		[TestMethod]
        public void It_should_be_possible_to_Locate_File_Base()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			PageLocatorService target = new PageLocatorService(workingPath);
            var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			// Base functionalities
            Assert.AreEqual(target.Locate("/").File.FullPath, Path.Combine(appPath, "index.xrc"));
            Assert.AreEqual(target.Locate("/").CanonicalVirtualUrl, "~/");
			Assert.AreEqual(target.Locate("/").File.VirtualPath, "~/samplewebsite1/index.xrc");
			Assert.AreEqual(target.Locate("/").File.Parent.VirtualPath, "~/samplewebsite1/");
			Assert.AreEqual(target.Locate("/about").File.FullPath, Path.Combine(appPath, "about.xrc"));
			Assert.AreEqual(target.Locate("/about#anchor").File.FullPath, Path.Combine(appPath, "about.xrc"));
			Assert.AreEqual(target.Locate("/ABOUT").File.FullPath, Path.Combine(appPath, "about.xrc"));
			Assert.AreEqual(target.Locate("/about").CanonicalVirtualUrl, "~/about");
			Assert.AreEqual(target.Locate("/about").File.VirtualPath, "~/samplewebsite1/about.xrc");
			Assert.AreEqual(target.Locate("/about").File.Parent.VirtualPath, "~/samplewebsite1/");
			Assert.AreEqual(target.Locate("/index").CanonicalVirtualUrl, "~/");
			Assert.AreEqual(target.Locate("/news/").File.FullPath, Path.Combine(appPath, @"news\index.xrc"));
			Assert.AreEqual(target.Locate("/news/?param=test").File.FullPath, Path.Combine(appPath, @"news\index.xrc"));
			Assert.AreEqual(target.Locate("/athletes").File.FullPath, Path.Combine(appPath, @"athletes\index.xrc"));
			Assert.AreEqual(target.Locate("/athletes").CanonicalVirtualUrl, "~/athletes/");
			Assert.AreEqual(target.Locate("/aTHletes").File.VirtualPath, "~/samplewebsite1/athletes/index.xrc");
			Assert.AreEqual(target.Locate("/aTHletes").File.Parent.VirtualPath, "~/samplewebsite1/athletes/");
			Assert.AreEqual(target.Locate("/ATHLETES").CanonicalVirtualUrl, "~/athletes/");
			Assert.AreEqual(target.Locate("/ATHLETES/indeX").CanonicalVirtualUrl, "~/athletes/");
			Assert.AreEqual(target.Locate("").File.FullPath, Path.Combine(appPath, "index.xrc"));
			Assert.AreEqual(target.Locate("athletes").File.FullPath, Path.Combine(appPath, @"athletes\index.xrc"));
        }

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_With_Parameters()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			PageLocatorService target = new PageLocatorService(workingPath);
			var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			// Dynamic pages
			Assert.AreEqual(target.Locate("/athletes/totti").File.FullPath, Path.Combine(appPath, @"athletes\{athleteid}\index.xrc"));
			Assert.AreEqual(target.Locate("/athletes/totti").UrlSegmentsParameters["athleteid"], "totti");
			Assert.AreEqual(target.Locate("/athletes/ToTTi").UrlSegmentsParameters["athleteid"], "totti");
			Assert.AreEqual(target.Locate("/teams/torino").File.FullPath, Path.Combine(appPath, @"teams\{teamid}\index.xrc"));
			Assert.AreEqual(target.Locate("/teams/torino").CanonicalVirtualUrl, "~/teams/torino/");
			Assert.AreEqual(target.Locate("/teams/torino").File.VirtualPath, "~/samplewebsite1/teams/{teamid}/index.xrc");
			Assert.AreEqual(target.Locate("/teams/torino").File.Parent.VirtualPath, "~/samplewebsite1/teams/{teamid}/");
			Assert.AreEqual(target.Locate("/teams/veroNA").File.VirtualPath, "~/samplewebsite1/teams/{teamid}/index.xrc");
			Assert.AreEqual(target.Locate("/teams/veroNA").File.Parent.VirtualPath, "~/samplewebsite1/teams/{teamid}/");
			Assert.AreEqual(target.Locate("/teams/torino/matches").File.FullPath, Path.Combine(appPath, @"teams\{teamid}\matches.xrc"));
			Assert.AreEqual(target.Locate("/TEAMS/TORINO/MATCHES").CanonicalVirtualUrl, "~/teams/torino/matches");
			Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["teamid"], "torino");
			Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["playerid"], "cravero");
			Assert.AreEqual(target.Locate("/teams/torino/matches.xrc").UrlSegmentsParameters["playerid"], "matches.xrc");

			Assert.AreEqual(target.Locate("/photos/photo5").File.FullPath, Path.Combine(appPath, @"photos\photo{id}\index.xrc"));
			Assert.AreEqual(target.Locate("/photos/photo5").UrlSegmentsParameters["id"], "5");

			Assert.AreEqual(target.Locate("/photos/569.95sport").File.FullPath, Path.Combine(appPath, @"photos\{id}sport\index.xrc"));
			Assert.AreEqual(target.Locate("/photos/569.95sport").UrlSegmentsParameters["id"], "569.95");
		}

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_With_Parameters_Catch_All()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			PageLocatorService target = new PageLocatorService(workingPath);
			var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			Assert.AreEqual(target.Locate("/news/sport/basket/2012").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
			Assert.AreEqual(target.Locate("/news/sport/basket/2012").UrlSegmentsParameters["id"], "sport/basket/2012");
			Assert.AreEqual(target.Locate("/news/sport/basket/2012").CanonicalVirtualUrl, "~/news/sport/basket/2012/");

			Assert.AreEqual(target.Locate("/news/4658135").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
			Assert.AreEqual(target.Locate("/news/4658135").UrlSegmentsParameters["id"], "4658135");
			Assert.AreEqual(target.Locate("/news/4658135").CanonicalVirtualUrl, "~/news/4658135/");

			Assert.AreEqual(target.Locate("/news/4658135/").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
			Assert.AreEqual(target.Locate("/news/4658135/").UrlSegmentsParameters["id"], "4658135");
			Assert.AreEqual(target.Locate("/news/4658135/").CanonicalVirtualUrl, "~/news/4658135/");

			Assert.AreEqual(target.Locate("/news/4658135/test/test/").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
			Assert.AreEqual(target.Locate("/news/4658135/test/test/").UrlSegmentsParameters["id"], "4658135/test/test");
			Assert.AreEqual(target.Locate("/news/4658135/test/test/").CanonicalVirtualUrl, "~/news/4658135/test/test/");

			Assert.AreEqual(target.Locate("/docs/4658135").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
			Assert.AreEqual(target.Locate("/docs/4658135").UrlSegmentsParameters["page"], "4658135");
			Assert.AreEqual(target.Locate("/docs/4658135").CanonicalVirtualUrl, "~/docs/4658135");

			Assert.AreEqual(target.Locate("/docs/4658135/").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
			Assert.AreEqual(target.Locate("/docs/4658135/").UrlSegmentsParameters["page"], "4658135");
			Assert.AreEqual(target.Locate("/docs/4658135/").CanonicalVirtualUrl, "~/docs/4658135");

			Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
			Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").UrlSegmentsParameters["page"], "4658135/test/long/url/index.htm");
			Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").CanonicalVirtualUrl, "~/docs/4658135/test/long/url/index.htm");
		}

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_Advanced()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			PageLocatorService target = new PageLocatorService(workingPath);
			var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			// Default page index and etensions
			Assert.AreEqual(target.Locate("/athletes/totti/index").File.FullPath, Path.Combine(appPath, @"athletes\{athleteid}\index.xrc"));

			// folder config file
			Assert.AreEqual(target.Locate("/athletes").File.Parent.GetConfigFile(), Path.Combine(appPath, @"athletes\xrc.config"));
			Assert.AreEqual(target.Locate("/teams").File.Parent.GetConfigFile(), null);

			// default layout resolution
			Assert.AreEqual(target.Locate("/about").File.Parent.SearchLayout().FullName, "~/shared/_layout");
			Assert.AreEqual(target.Locate("/teams/torino").File.Parent.SearchLayout().FullName, "~/teams/{teamid}/_layout");

			// File not found == null
			Assert.IsNull(target.Locate("notvalid"));
			Assert.IsNull(target.Locate("/notvalid"));
			Assert.IsNull(target.Locate("/athletes/totti/notfound"));
			Assert.IsNull(target.Locate("/athletes/totti/index.xrc"));
		}

		[TestMethod]
		public void It_should_not_be_possible_to_use_invalid_url()
		{
			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			PageLocatorService target = new PageLocatorService(workingPath);
			var appPath = workingPath.PhysicalPath.ToLowerInvariant();

			// Absolute uri not supported
			TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/absoluteuri"));
			TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/"));
			TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1.com"));
		}
	}
}
