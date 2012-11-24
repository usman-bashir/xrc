using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
	public interface ISitesConfig
    {
		IEnumerable<Sites.ISiteConfiguration> Sites
        {
            get;
        }
	}
}
