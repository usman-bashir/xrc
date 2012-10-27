using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xrc
{
	[TestClass]
    public class UriExtensions_Test
    {
		[TestMethod]
        public void Combine_Uri()
        {
			Assert.AreEqual(new Uri("http://www.google.com/index"), 
						UriExtensions.Combine(new Uri("http://www.google.com"), new Uri("index", UriKind.Relative)));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder"), new Uri("index", UriKind.Relative)));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/"), new Uri("/index", UriKind.Relative)));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/sub1/sub2/"), new Uri("../../index", UriKind.Relative)));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/sub1/sub2"), new Uri("../../index", UriKind.Relative)));
			Assert.AreEqual(new Uri("/folder/index", UriKind.Relative),
						UriExtensions.Combine(new Uri("/folder", UriKind.Relative), new Uri("index", UriKind.Relative)));
			Assert.AreEqual(new Uri("/folder/index", UriKind.Relative),
						UriExtensions.Combine(new Uri("/folder/sub1/sub2", UriKind.Relative), new Uri("../../index", UriKind.Relative)));
		}

		[TestMethod]
		public void Combine_Uri_String()
		{
			Assert.AreEqual(new Uri("http://www.google.com/index"),
						UriExtensions.Combine(new Uri("http://www.google.com"), "index"));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder"), "index"));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/"), "/index"));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/sub1/sub2/"), "../../index"));
			Assert.AreEqual(new Uri("http://www.google.com/folder/index"),
						UriExtensions.Combine(new Uri("http://www.google.com/folder/sub1/sub2"), "../../index"));
			Assert.AreEqual(new Uri("/folder/index", UriKind.Relative),
						UriExtensions.Combine(new Uri("/folder/sub1/sub2", UriKind.Relative), "../../index"));
		}

		[TestMethod]
		public void Combine_String()
		{
			Assert.AreEqual("http://www.google.com/index",
						UriExtensions.Combine("http://www.google.com", "index"));
			Assert.AreEqual("http://www.google.com/folder/index",
						UriExtensions.Combine("http://www.google.com/folder", "index"));
			Assert.AreEqual("http://www.google.com/folder/index",
						UriExtensions.Combine("http://www.google.com/folder/", "/index"));
			Assert.AreEqual("http://www.google.com/folder/index",
						UriExtensions.Combine("http://www.google.com/folder/sub1/sub2/", "../../index"));
			Assert.AreEqual("http://www.google.com/folder/index",
						UriExtensions.Combine("http://www.google.com/folder/sub1/sub2", "../../index"));
			Assert.AreEqual("/folder/index",
						UriExtensions.Combine("/folder/sub1/sub2", "../../index"));
		}

		[TestMethod]
		public void IsBaseOfWithPath()
        {
			Assert.AreEqual(false, new Uri("http://contoso.com/path").IsBaseOfWithPath(new Uri("http://contoso.com")));
			Assert.AreEqual(true, new Uri("http://contoso.com").IsBaseOfWithPath(new Uri("http://contoso.com/path")));
			Assert.AreEqual(false, new Uri("http://contoso.com/path/").IsBaseOfWithPath(new Uri("http://contoso.com/")));
			Assert.AreEqual(true, new Uri("http://contoso.com/").IsBaseOfWithPath(new Uri("http://contoso.com/path/")));

			Assert.AreEqual(true, new Uri("/base/", UriKind.Relative).IsBaseOfWithPath(new Uri("/base/path/", UriKind.Relative)));
			Assert.AreEqual(true, new Uri("/base", UriKind.Relative).IsBaseOfWithPath(new Uri("/base/path/", UriKind.Relative)));
			Assert.AreEqual(false, new Uri("/test/", UriKind.Relative).IsBaseOfWithPath(new Uri("/base/path/", UriKind.Relative)));
			Assert.AreEqual(true, new Uri("/", UriKind.Relative).IsBaseOfWithPath(new Uri("/base/path/", UriKind.Relative)));
			Assert.AreEqual(true, new Uri("", UriKind.Relative).IsBaseOfWithPath(new Uri("/base/path/", UriKind.Relative)));
		}

		[TestMethod]
		public void MakeRelativeUriEx()
        {
			Assert.AreEqual(new Uri("", UriKind.Relative), new Uri("http://contoso.com/vpath").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));
			Assert.AreEqual(new Uri("index", UriKind.Relative), new Uri("http://contoso.com/vpath/index").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));
			Assert.AreEqual(new Uri("index/page", UriKind.Relative), new Uri("http://contoso.com/vpath/index/page").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));

			Assert.AreEqual(new Uri("vpath", UriKind.Relative), new Uri("/base/vpath", UriKind.Relative).MakeRelativeUriEx(new Uri("/base/", UriKind.Relative)));
			Assert.AreEqual(new Uri("vpath", UriKind.Relative), new Uri("/base/vpath", UriKind.Relative).MakeRelativeUriEx(new Uri("/base", UriKind.Relative)));
			Assert.AreEqual(new Uri("vpath/", UriKind.Relative), new Uri("/base/vpath/", UriKind.Relative).MakeRelativeUriEx(new Uri("/base/", UriKind.Relative)));
			Assert.AreEqual(new Uri("/test/vpath", UriKind.Relative), new Uri("/test/vpath", UriKind.Relative).MakeRelativeUriEx(new Uri("/base/", UriKind.Relative)));
			Assert.AreEqual(new Uri("/base/vpath", UriKind.Relative), new Uri("/base/vpath", UriKind.Relative).MakeRelativeUriEx(new Uri("/", UriKind.Relative)));
			Assert.AreEqual(new Uri("/base/vpath", UriKind.Relative), new Uri("/base/vpath", UriKind.Relative).MakeRelativeUriEx(new Uri("", UriKind.Relative)));
		}

		[TestMethod]
		public void ToLower()
        {
			Assert.AreEqual(new Uri("http://www.google.com"), new Uri("http://www.GOOGLE.com").ToLower());
        }

		[TestMethod]
		public void AppendTrailingSlash()
        {
			Assert.AreEqual(new Uri("http://www.google.com/"), new Uri("http://www.google.com").AppendTrailingSlash());
			Assert.AreEqual(new Uri("http://www.google.com/"), new Uri("http://www.google.com/").AppendTrailingSlash());
		}

		[TestMethod]
		public void AppendTrailingSlash_String()
		{
			Assert.AreEqual("http://www.google.com/", UriExtensions.AppendTrailingSlash("http://www.google.com"));
			Assert.AreEqual("http://www.google.com/", UriExtensions.AppendTrailingSlash("http://www.google.com/"));
			Assert.AreEqual("test/", UriExtensions.AppendTrailingSlash("test"));
			Assert.AreEqual("test/", UriExtensions.AppendTrailingSlash("test/"));
		}

		[TestMethod]
		public void RemoveTrailingSlash_String()
		{
			Assert.AreEqual("http://www.google.com", UriExtensions.RemoveTrailingSlash("http://www.google.com"));
			Assert.AreEqual("http://www.google.com", UriExtensions.RemoveTrailingSlash("http://www.google.com/"));
		}

		[TestMethod]
		public void RemoveHeadSlash_String()
		{
			Assert.AreEqual("test", UriExtensions.RemoveHeadSlash("/test"));
			Assert.AreEqual("test", UriExtensions.RemoveHeadSlash("test"));
		}

		[TestMethod]
		public void ToSecure()
		{
			Assert.AreEqual(new Uri("https://www.google.com/"), new Uri("http://www.google.com").ToSecure());
		}

		[TestMethod]
		public void GetPath()
		{
			Assert.AreEqual("/index/page", new Uri("/index/page?p1=v1", UriKind.Relative).GetPath());
			Assert.AreEqual("/index/page", new Uri("/index/page#anchor", UriKind.Relative).GetPath());
			Assert.AreEqual("/index/page", new Uri("http://www.google.com/index/page?p1=v1").GetPath());
			Assert.AreEqual("/index/page", new Uri("http://www.google.com/index/page#anchor").GetPath());

			// GetPath doesn't encode, this is a different behavior between Uri.GetLeftPart
			Assert.AreEqual("/index/page{test}+", new Uri("/index/page{test}+", UriKind.Relative).GetPath());
		}

		[TestMethod]
		public void GetQuery()
		{
			Assert.AreEqual(string.Empty, new Uri("/index/page", UriKind.Relative).GetQuery());
			Assert.AreEqual(string.Empty, new Uri("http://www.google.com/index/page").GetQuery());
			Assert.AreEqual(string.Empty, new Uri("/index/page?", UriKind.Relative).GetQuery());
			Assert.AreEqual("#", new Uri("/index/page#", UriKind.Relative).GetQuery());
			Assert.AreEqual("p1=v1", new Uri("/index/page?p1=v1", UriKind.Relative).GetQuery());
			Assert.AreEqual("p1=v1#anchor", UriExtensions.GetQuery("index/page?p1=v1#anchor"));
			Assert.AreEqual("#anchor", new Uri("/index/page#anchor", UriKind.Relative).GetQuery());
			Assert.AreEqual("p1=v1", new Uri("http://www.google.com/index/page?p1=v1").GetQuery());
			Assert.AreEqual("#anchor", new Uri("http://www.google.com/index/page#anchor").GetQuery());
			Assert.AreEqual("p1=v1#anchor", new Uri("http://www.google.com/index/page?p1=v1#anchor").GetQuery());

			// GetPath doesn't encode, this is a different behavior between Uri.GetLeftPart
			Assert.AreEqual(string.Empty, new Uri("/index/page{test}+", UriKind.Relative).GetQuery());
		}

		[TestMethod]
		public void BuildVirtualPath()
		{
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/", "test"));
			Assert.AreEqual("~/base/test/", UriExtensions.BuildVirtualPath("~/base/", "test/"));
			Assert.AreEqual("/test", UriExtensions.BuildVirtualPath("~/base/", "/test"));
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/index", "test"));
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/f1/f2/index", "../../test"));
			Assert.AreEqual("~/test", UriExtensions.BuildVirtualPath("~/base/", "~/test"));
		}

		[TestMethod]
		public void AppRelativeUrlToRelativeUrl()
		{
			Assert.AreEqual("/base/test", UriExtensions.AppRelativeUrlToRelativeUrl("~/test", "/base"));
			Assert.AreEqual("/base/test", UriExtensions.AppRelativeUrlToRelativeUrl("~/test", "base"));

			Assert.AreEqual("/base/test", UriExtensions.AppRelativeUrlToRelativeUrl("test", "/base"));
			Assert.AreEqual("/test", UriExtensions.AppRelativeUrlToRelativeUrl("/test", "/base"));
		}

		[TestMethod]
		public void RelativeUrlToAppRelativeUrl()
		{
			Assert.AreEqual("~/test", UriExtensions.RelativeUrlToAppRelativeUrl("/base/test", new Uri("/base", UriKind.Relative)));
			Assert.AreEqual("~/base/test", UriExtensions.RelativeUrlToAppRelativeUrl("/base/test", new Uri("", UriKind.Relative)));
			Assert.AreEqual("~/base/test", UriExtensions.RelativeUrlToAppRelativeUrl("/base/test", new Uri("", UriKind.Relative)));
		}

		[TestMethod]
		public void IsAppRelativeVirtualUrl()
		{
			Assert.AreEqual(true, UriExtensions.IsAppRelativeVirtualUrl("~/test"));
			Assert.AreEqual(true, UriExtensions.IsAppRelativeVirtualUrl("~/test/index"));
			Assert.AreEqual(true, UriExtensions.IsAppRelativeVirtualUrl("~/"));
			Assert.AreEqual(true, UriExtensions.IsAppRelativeVirtualUrl("~"));
			Assert.AreEqual(false, UriExtensions.IsAppRelativeVirtualUrl("/test/index"));
			Assert.AreEqual(false, UriExtensions.IsAppRelativeVirtualUrl("test/index"));
			Assert.AreEqual(false, UriExtensions.IsAppRelativeVirtualUrl("http://www.google.com/index/page?p1=v1#anchor"));
		}
    }
}
