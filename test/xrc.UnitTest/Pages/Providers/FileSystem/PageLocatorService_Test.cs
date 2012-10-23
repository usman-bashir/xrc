using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using xrc.Configuration;
using Moq;
using xrc.Pages.Providers.Common;

namespace xrc.Pages.Providers.FileSystem
{
	class TestPageStructure : IPageStructureService
	{
		public XrcItem GetRoot()
		{
			return XrcItem.NewRoot("~/",
					XrcItem.NewXrcFile("index.xrc"),
					XrcItem.NewXrcFile("about.xrc"),
					XrcItem.NewDirectory("news",
						XrcItem.NewXrcFile("index.xrc")
					),
					XrcItem.NewDirectory("athletes",
						XrcItem.NewXrcFile("index.xrc"),
						XrcItem.NewConfigFile(),
						XrcItem.NewDirectory("{athleteid}",
							XrcItem.NewXrcFile("index.xrc"),
							XrcItem.NewXrcFile("bio.xrc")
						)
					)
				);
		}
	}

	class TestPageStructure_With_VirtualDir : IPageStructureService
	{
		public XrcItem GetRoot()
		{
			return XrcItem.NewRoot("~/xrcroot",
					XrcItem.NewXrcFile("index.xrc"),
					XrcItem.NewXrcFile("about.xrc")
				);
		}
	}

	[TestClass]
	public class PageLocatorService_Test
	{
		PageLocatorResult Locate(string url)
		{
			PageLocatorService target = new PageLocatorService(new TestPageStructure());
			return target.Locate(new XrcUrl(url));
		}

		PageLocatorResult Locate_with_VirtualDir(string url)
		{
			PageLocatorService target = new PageLocatorService(new TestPageStructure_With_VirtualDir());
			return target.Locate(new XrcUrl(url));
		}

		[TestMethod]
		public void Locate_Base_Functionalities()
		{
			Assert.AreEqual("~/about", Locate("~/about").Url.ToString());
			Assert.AreEqual("~/about.xrc", Locate("~/about").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_With_Virtual_Directory_Base_Functionalities()
		{
			Assert.AreEqual("~/about", Locate_with_VirtualDir("~/about").Url.ToString());
			Assert.AreEqual("~/xrcroot/about.xrc", Locate_with_VirtualDir("~/about").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Index_Page_without_specify_it()
		{
			Assert.AreEqual("~/", Locate("~/").Url.ToString());
			Assert.AreEqual("~/index.xrc", Locate("~/").Item.ResourceLocation);

			Assert.AreEqual("~/news/index.xrc", Locate("~/news").Item.ResourceLocation);
			Assert.AreEqual("~/news/index.xrc", Locate("~/news/").Item.ResourceLocation);

			Assert.AreEqual("~/news/index.xrc", Locate("~/news/?query#anchor").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Index_Page_specify_it()
		{
			Assert.AreEqual("~/", Locate("~/index").Url.ToString());
		}

		[TestMethod]
		public void Locate_Is_Case_Insensitive()
		{
			Assert.AreEqual("~/about.xrc", Locate("~/ABOUT").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Url_can_contains_query()
		{
			Assert.AreEqual("~/about.xrc", Locate("~/about?query=x").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Url_can_contains_anchor()
		{
			Assert.AreEqual("~/about.xrc", Locate("~/about#anchor").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Url_with_segment_parameters()
		{
			Assert.AreEqual("~/athletes/{athleteid}/index.xrc", Locate("~/athletes/totti").Item.ResourceLocation);
			Assert.AreEqual("totti", Locate("~/athletes/totti").UrlSegmentsParameters["athleteid"]);
			Assert.AreEqual("~/athletes/{athleteid}/bio.xrc", Locate("~/athletes/totti/bio").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Url_with_segment_parameters_case_insensitive()
		{
			Assert.AreEqual("~/athletes/{athleteid}/index.xrc", Locate("~/athletes/TOTTI").Item.ResourceLocation);
			Assert.AreEqual("totti", Locate("~/athletes/TOTTI").UrlSegmentsParameters["athleteid"]);
		}

		//[TestMethod]
		//public void It_should_be_possible_to_Locate_File_With_Parameters()
		//{
		//    Assert.AreEqual(target.Locate("/teams/torino/matches").File.FullPath, Path.Combine(appPath, @"teams\{teamid}\matches.xrc"));
		//    Assert.AreEqual(target.Locate("/TEAMS/TORINO/MATCHES").CanonicalVirtualUrl, "~/teams/torino/matches");
		//    Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["teamid"], "torino");
		//    Assert.AreEqual(target.Locate("/teams/torino/cravero").UrlSegmentsParameters["playerid"], "cravero");
		//    Assert.AreEqual(target.Locate("/teams/torino/matches.xrc").UrlSegmentsParameters["playerid"], "matches.xrc");

		//    Assert.AreEqual(target.Locate("/photos/photo5").File.FullPath, Path.Combine(appPath, @"photos\photo{id}\index.xrc"));
		//    Assert.AreEqual(target.Locate("/photos/photo5").UrlSegmentsParameters["id"], "5");

		//    Assert.AreEqual(target.Locate("/photos/569.95sport").File.FullPath, Path.Combine(appPath, @"photos\{id}sport\index.xrc"));
		//    Assert.AreEqual(target.Locate("/photos/569.95sport").UrlSegmentsParameters["id"], "569.95");
		//}

		//[TestMethod]
		//public void It_should_be_possible_to_Locate_File_With_Parameters_Catch_All()
		//{
		//    var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
		//    PageLocatorService target = new PageLocatorService(workingPath);
		//    var appPath = workingPath.PhysicalPath.ToLowerInvariant();

		//    Assert.AreEqual(target.Locate("/news/sport/basket/2012").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
		//    Assert.AreEqual(target.Locate("/news/sport/basket/2012").UrlSegmentsParameters["id"], "sport/basket/2012");
		//    Assert.AreEqual(target.Locate("/news/sport/basket/2012").CanonicalVirtualUrl, "~/news/sport/basket/2012/");

		//    Assert.AreEqual(target.Locate("/news/4658135").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
		//    Assert.AreEqual(target.Locate("/news/4658135").UrlSegmentsParameters["id"], "4658135");
		//    Assert.AreEqual(target.Locate("/news/4658135").CanonicalVirtualUrl, "~/news/4658135/");

		//    Assert.AreEqual(target.Locate("/news/4658135/").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
		//    Assert.AreEqual(target.Locate("/news/4658135/").UrlSegmentsParameters["id"], "4658135");
		//    Assert.AreEqual(target.Locate("/news/4658135/").CanonicalVirtualUrl, "~/news/4658135/");

		//    Assert.AreEqual(target.Locate("/news/4658135/test/test/").File.FullPath, Path.Combine(appPath, @"news\{id_catch-all}\index.xrc"));
		//    Assert.AreEqual(target.Locate("/news/4658135/test/test/").UrlSegmentsParameters["id"], "4658135/test/test");
		//    Assert.AreEqual(target.Locate("/news/4658135/test/test/").CanonicalVirtualUrl, "~/news/4658135/test/test/");

		//    Assert.AreEqual(target.Locate("/docs/4658135").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
		//    Assert.AreEqual(target.Locate("/docs/4658135").UrlSegmentsParameters["page"], "4658135");
		//    Assert.AreEqual(target.Locate("/docs/4658135").CanonicalVirtualUrl, "~/docs/4658135");

		//    Assert.AreEqual(target.Locate("/docs/4658135/").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
		//    Assert.AreEqual(target.Locate("/docs/4658135/").UrlSegmentsParameters["page"], "4658135");
		//    Assert.AreEqual(target.Locate("/docs/4658135/").CanonicalVirtualUrl, "~/docs/4658135");

		//    Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").File.FullPath, Path.Combine(appPath, @"docs\{page_catch-all}.xrc"));
		//    Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").UrlSegmentsParameters["page"], "4658135/test/long/url/index.htm");
		//    Assert.AreEqual(target.Locate("/docs/4658135/test/long/url/index.htm").CanonicalVirtualUrl, "~/docs/4658135/test/long/url/index.htm");
		//}

		//[TestMethod]
		//public void It_should_be_possible_to_Locate_File_Advanced()
		//{
		//    var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
		//    PageLocatorService target = new PageLocatorService(workingPath);
		//    var appPath = workingPath.PhysicalPath.ToLowerInvariant();

		//    // Default page index and etensions
		//    Assert.AreEqual(target.Locate("/athletes/totti/index").File.FullPath, Path.Combine(appPath, @"athletes\{athleteid}\index.xrc"));

		//    // folder config file
		//    Assert.AreEqual(target.Locate("/athletes").File.Parent.GetConfigFile(), Path.Combine(appPath, @"athletes\xrc.config"));
		//    Assert.AreEqual(target.Locate("/teams").File.Parent.GetConfigFile(), null);

		//    // default layout resolution
		//    Assert.AreEqual(target.Locate("/about").File.Parent.SearchLayout().FullName, "~/shared/_layout");
		//    Assert.AreEqual(target.Locate("/teams/torino").File.Parent.SearchLayout().FullName, "~/teams/{teamid}/_layout");

		//    // File not found == null
		//    Assert.IsNull(target.Locate("notvalid"));
		//    Assert.IsNull(target.Locate("/notvalid"));
		//    Assert.IsNull(target.Locate("/athletes/totti/notfound"));
		//    Assert.IsNull(target.Locate("/athletes/totti/index.xrc"));
		//}

		//[TestMethod]
		//public void It_should_not_be_possible_to_use_invalid_url()
		//{
		//    var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
		//    PageLocatorService target = new PageLocatorService(workingPath);
		//    var appPath = workingPath.PhysicalPath.ToLowerInvariant();

		//    // Absolute uri not supported
		//    TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/absoluteuri"));
		//    TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1/"));
		//    TestHelper.Throws<UriFormatException>(() => target.Locate("http://server1.com"));
		//}
	}
}
