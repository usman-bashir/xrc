using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace xrc.Configuration
{
	public class AspNetHostingConfig : IHostingConfig
	{
		public Uri WebSiteVirtualDirectory
		{
			get { return new Uri(UriExtensions.Combine("/", System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath), UriKind.Relative); }
		}
		public Uri AppRelativeUrlToRelativeUrl(string url)
		{
			return new Uri(UriExtensions.AppRelativeUrlToRelativeUrl(url, WebSiteVirtualDirectory.ToString()), UriKind.RelativeOrAbsolute);
		}
		public string RelativeUrlToAppRelativeUrl(Uri url)
		{
			return UriExtensions.RelativeUrlToAppRelativeUrl(url.ToString(), WebSiteVirtualDirectory);
		}
	}
}
