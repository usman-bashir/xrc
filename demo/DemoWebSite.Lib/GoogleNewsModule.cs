using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace DemoWebSite
{
    public class GoogleNewsModule
    {
        public string Search(string searchValue)
        {
            string url = string.Format("https://ajax.googleapis.com/ajax/services/search/news?v=1.0&q={0}", HttpUtility.UrlEncode(searchValue));

            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}