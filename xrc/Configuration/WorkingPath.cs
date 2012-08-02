using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public class WorkingPath
    {
        public WorkingPath(string virtualPath, string phisicalPath)
        {
            VirtualPath = virtualPath;
            PhysicalPath = phisicalPath;
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
