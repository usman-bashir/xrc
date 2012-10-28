using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace xrc.Configuration
{
	public class RootPathElement : ConfigurationElement, IFileSystemConfig
	{
		[ConfigurationProperty("virtualPath", IsRequired = true)]
		public string VirtualPath
		{
			get { return (string)this["virtualPath"]; }
			set { this["virtualPath"] = value; }
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
