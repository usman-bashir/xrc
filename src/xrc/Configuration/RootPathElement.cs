using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace xrc.Configuration
{
	public class RootPathElement : ConfigurationElement, IRootPathConfig
	{
		[ConfigurationProperty("virtualPath", IsRequired = true)]
		public string VirtualPath
		{
			get { return (string)this["virtualPath"]; }
			set { this["virtualPath"] = value; }
		}

		string IRootPathConfig.VirtualPath
		{
			get { return VirtualPath; }
		}

		string IRootPathConfig.PhysicalPath
		{
			get { return System.Web.Hosting.HostingEnvironment.MapPath(VirtualPath); }
		}

		string IRootPathConfig.MapPath(string virtualPath)
		{
			return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
		}

		public Uri AppRelativeUrlToRelativeUrl(string url)
		{
			return new Uri(UriExtensions.AppRelativeUrlToRelativeUrl(url, VirtualPath), UriKind.RelativeOrAbsolute);
		}
	}
}
