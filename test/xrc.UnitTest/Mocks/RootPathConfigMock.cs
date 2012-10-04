using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using xrc.Configuration;

namespace xrc.Mocks
{
	public class RootPathConfigMock : IRootPathConfig
	{
		public RootPathConfigMock(string virtualPath, string physicalPath)
		{
			VirtualPath = virtualPath;
			PhysicalPath = physicalPath;
		}

		public string VirtualPath
		{
			get;
			set;
		}
		public string PhysicalPath
		{
			get;
			set;
		}

		string IRootPathConfig.VirtualPath
		{
			get { return VirtualPath; }
		}

		string IRootPathConfig.PhysicalPath
		{
			get { return PhysicalPath; }
		}


		public string MapPath(string virtualPath)
		{
			throw new NotImplementedException();
		}
	}
}
