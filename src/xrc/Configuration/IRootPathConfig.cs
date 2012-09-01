using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public interface IRootPathConfig
    {
        string VirtualPath
        {
            get;
        }

        string PhysicalPath
        {
            get;
        }
    }
}
