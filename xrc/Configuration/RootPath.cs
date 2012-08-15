using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public class RootPath
    {
		public RootPath(string virtualPath, string physicalPath)
        {
            VirtualPath = virtualPath;
			PhysicalPath = physicalPath;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string PhysicalPath
        {
            get;
            private set;
        }
    }
}
