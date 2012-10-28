using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using xrc.Configuration;
using Moq;

namespace xrc.Pages.Providers.Common
{
	[TestClass]
	public class PageLocatorService_Test
	{
		PageLocatorResult Locate(string url)
		{
			var target = new PageLocatorService(new TestPageStructure());
			return target.Locate(new XrcUrl(url));
		}

		PageLocatorResult Locate_with_VirtualDir(string url)
		{
			var target = new PageLocatorService(new TestPageStructure_With_VirtualDir());
			return target.Locate(new XrcUrl(url));
		}

		[TestMethod]
		public void Locate_Base_Functionalities()
		{
			Assert.AreEqual("~/about.xrc", Locate("~/about").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Resource_Case_is_maintaned_but_url_is_lowercase()
		{
			Assert.AreEqual("~/ConTact.xrc", Locate("~/ConTact").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_With_Virtual_Directory_Base_Functionalities()
		{
			Assert.AreEqual("~/xrcroot/about.xrc", Locate_with_VirtualDir("~/about").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Index_Page_without_specify_it()
		{
			Assert.AreEqual("~/index.xrc", Locate("~/").Item.ResourceLocation);

			Assert.AreEqual("~/news/index.xrc", Locate("~/news").Item.ResourceLocation);
			Assert.AreEqual("~/news/index.xrc", Locate("~/news/").Item.ResourceLocation);

			Assert.AreEqual("~/news/index.xrc", Locate("~/news/?query#anchor").Item.ResourceLocation);
		}

		[TestMethod]
		public void Locate_Index_Page_specify_it()
		{
			Assert.AreEqual("~/index.xrc", Locate("~/index").Item.ResourceLocation);
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
		public void Locate_Url_with_segment_parameters_static_file_precedence()
		{
			// static files (matches) have precedence over dynamic {playerid}
			Assert.AreEqual("~/teams/{teamid}/matches.xrc", Locate("~/teams/torino/matches").Item.ResourceLocation);
			Assert.AreEqual("torino", Locate("~/teams/torino/matches").UrlSegmentsParameters["teamid"]);
		}

		[TestMethod]
		public void Locate_Url_with_2_segment_parameters()
		{
			// url with two url parameter (teamid, playerid)
			Assert.AreEqual("~/teams/{teamid}/{playerid}/index.xrc", Locate("~/teams/torino/cravero").Item.ResourceLocation);
			Assert.AreEqual("torino", Locate("~/teams/torino/cravero").UrlSegmentsParameters["teamid"]);
			Assert.AreEqual("cravero", Locate("~/teams/torino/cravero").UrlSegmentsParameters["playerid"]);

			// In this case I pass as a playerId the name of a file, the systm just consider it the player
			Assert.AreEqual("matches.xrc", Locate("~/teams/torino/matches.xrc").UrlSegmentsParameters["playerid"]);
		}

		[TestMethod]
		public void Locate_Url_with_segment_parameters_case_insensitive()
		{
			Assert.AreEqual("~/athletes/{athleteid}/index.xrc", Locate("~/ATHLETES/TOTTI").Item.ResourceLocation);
			Assert.AreEqual("totti", Locate("~/athletes/TOTTI").UrlSegmentsParameters["athleteid"]);
		}

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_With_Parameters_prefix_suffix()
		{
			Assert.AreEqual("~/photos/photo{id}/index.xrc", Locate("~/photos/photo5/index").Item.ResourceLocation);
			Assert.AreEqual("5", Locate("~/photos/photo5/index").UrlSegmentsParameters["id"]);

			Assert.AreEqual("~/photos/{id}sport/index.xrc", Locate("~/photos/845sport/index").Item.ResourceLocation);
			Assert.AreEqual("845", Locate("~/photos/845sport/index").UrlSegmentsParameters["id"]);
		}

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_With_Parameters_Catch_All_Directory()
		{
			Assert.AreEqual("~/news/{id_catch-all}/index.xrc", Locate("~/news/sport/basket/2012").Item.ResourceLocation);
			Assert.AreEqual("sport/basket/2012", Locate("~/news/sport/basket/2012").UrlSegmentsParameters["id"]);

			Assert.AreEqual("~/news/{id_catch-all}/index.xrc", Locate("~/news/4658135").Item.ResourceLocation);
			Assert.AreEqual("4658135", Locate("~/news/4658135").UrlSegmentsParameters["id"]);
			Assert.AreEqual("~/news/{id_catch-all}/index.xrc", Locate("~/news/4658135/").Item.ResourceLocation);
			Assert.AreEqual("4658135", Locate("~/news/4658135/").UrlSegmentsParameters["id"]);
		}

		[TestMethod]
		public void It_should_be_possible_to_Locate_File_With_Parameters_Catch_All_File()
		{
			Assert.AreEqual("~/docs/{page_CATCH-ALL}.xrc", Locate("~/docs/4658135").Item.ResourceLocation);
			Assert.AreEqual("4658135", Locate("~/docs/4658135").UrlSegmentsParameters["page"]);

			Assert.AreEqual("~/docs/{page_CATCH-ALL}.xrc", Locate("~/docs/4658135/").Item.ResourceLocation);
			Assert.AreEqual("4658135", Locate("~/docs/4658135/").UrlSegmentsParameters["page"]);

			Assert.AreEqual("~/docs/{page_CATCH-ALL}.xrc", Locate("~/docs/4658135/test/long/url/index.htm").Item.ResourceLocation);
			Assert.AreEqual("4658135/test/long/url/index.htm", Locate("~/docs/4658135/test/long/url/index.htm").UrlSegmentsParameters["page"]);
		}

		[TestMethod]
		public void File_not_found()
		{
		    // File not found == null
			Assert.IsNull(Locate("~/index.xrc"));
			Assert.IsNull(Locate("~/notvalid"));
		    Assert.IsNull(Locate("~/athletes/totti/notfound"));
		    Assert.IsNull(Locate("~/athletes/totti/index.xrc"));
		}

		[TestMethod]
		public void It_should_not_be_possible_to_use_invalid_url()
		{
			TestHelper.Throws<XrcException>(() => Locate("notvalid"));
			TestHelper.Throws<XrcException>(() => Locate("/notvalid"));
			TestHelper.Throws<XrcException>(() => Locate("~notvalid"));
			TestHelper.Throws<XrcException>(() => Locate("http://server1/absoluteuri"));
			TestHelper.Throws<XrcException>(() => Locate("http://server1/"));
			TestHelper.Throws<XrcException>(() => Locate("http://server1.com"));
		}
	}
}
