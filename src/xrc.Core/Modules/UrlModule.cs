﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    public class UrlModule : IUrlModule
    {
        readonly IContext _context;
		readonly Configuration.IHostingConfig _hostingConfiguration;

		public UrlModule(IContext context, Configuration.IHostingConfig hostingConfiguration)
        {
			_hostingConfiguration = hostingConfiguration;
            _context = context;
        }

		public string Content(string contentPath)
		{
			string appRelativeContent = _context.Page.GetAppRelativeUrl(contentPath);
			return _hostingConfiguration.AppRelativeUrlToRelativeUrl(appRelativeContent).ToString();
		}

		public string Content(string contentPathBase, string contentPath)
		{
			return UriExtensions.Combine(Content(contentPathBase), contentPath);
		}

		public string Current()
		{
			return _context.Request.Url.ToString();
		}

		public string Initiator()
		{
			return _context.GetInitiatorContext().Request.Url.ToString();
		}

		public bool IsBaseOf(string baseUrl, string url)
		{
			Uri baseUrlUri = new Uri(baseUrl);
			return baseUrlUri.IsBaseOfWithPath(new Uri(url));
		}
	}
}
