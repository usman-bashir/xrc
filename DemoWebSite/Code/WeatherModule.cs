using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml.Linq;

namespace DemoWebSite
{
    public class WeatherModule
    {
        public XDocument Get(string location)
        {
            XDocument xml = XDocument.Load("http://www.google.com/ig/api?weather=" + HttpUtility.UrlEncode(location));

            return xml;
        }
    }
}