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
        }

		[TestMethod]
		public void MakeRelativeUriEx()
        {
			Assert.AreEqual(new Uri("", UriKind.Relative), new Uri("http://contoso.com/vpath").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));
			Assert.AreEqual(new Uri("index", UriKind.Relative), new Uri("http://contoso.com/vpath/index").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));
			Assert.AreEqual(new Uri("index/page", UriKind.Relative), new Uri("http://contoso.com/vpath/index/page").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/")));
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
		public void BuildVirtualPath()
		{
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/", "test"));
			Assert.AreEqual("~/base/test/", UriExtensions.BuildVirtualPath("~/base/", "test/"));
			Assert.AreEqual("/test", UriExtensions.BuildVirtualPath("~/base/", "/test"));
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/index", "test"));
			Assert.AreEqual("~/base/test", UriExtensions.BuildVirtualPath("~/base/f1/f2/index", "../../test"));
			Assert.AreEqual("~/test", UriExtensions.BuildVirtualPath("~/base/", "~/test"));
		}
    }
}
