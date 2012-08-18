using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
	public interface IModuleConfig
    {
        ComponentCollection Modules
        {
            get;
        }
	}
}
