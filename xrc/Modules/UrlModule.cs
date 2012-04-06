using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    public class UrlModule : IUrlModule
    {
        private IContext _context;
        public UrlModule(IContext context)
        {
            _context = context;
        }

        public string Content(string contentPath)
        {
            var cfg = _context.Configuration;
            return cfg.UrlContent(contentPath, _context.Request.Url);
        }


        public string Content(string baseUri, string uri)
        {
            return UriExtensions.Combine(Content(baseUri), uri);
        }

        public string Content(string baseUri, string uri1, string uri2)
        {
            return UriExtensions.Combine(UriExtensions.Combine(Content(baseUri), uri1), uri2);
        }
    }
}
