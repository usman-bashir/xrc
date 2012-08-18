using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

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
	}
}
