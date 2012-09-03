using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml.Linq;

namespace DemoWebSite
{
    public class TwitterModule
    {
        public XDocument Search(string hashTag)
        {
			XDocument xml = XDocument.Load("http://search.twitter.com/search.rss?q=" + HttpUtility.UrlEncode(hashTag));

            return xml;
        }
    }
}