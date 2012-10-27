using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace xrc.Configuration
{
	// TODO Da implementare da classi diverse

	public class RootPathElement : ConfigurationElement, IHostingConfig, IFileSystemConfig
	{
		[ConfigurationProperty("virtualPath", IsRequired = true)]
		public string VirtualPath
		{
			get { return (string)this["virtualPath"]; }
			set { this["virtualPath"] = value; }
		}

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


		public string XrcRootVirtualPath
		{
			get { return VirtualPath; }
		}
		string IFileSystemConfig.MapPath(string virtualPath)
		{
			return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
		}
	}
}
