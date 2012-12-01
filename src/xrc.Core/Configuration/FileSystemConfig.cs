using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace xrc.Configuration
{
	public class FileSystemConfig : IFileSystemConfig
	{
		readonly XrcSection _xrcSection;
		public FileSystemConfig(XrcSection xrcSection)
		{
			_xrcSection = xrcSection;
		}

		public string XrcRootVirtualPath
		{
			get { return _xrcSection.RootPath.VirtualPath; }
		}
		public string MapPath(string virtualPath)
		{
			return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
		}
	}
}
