using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.SiteManager;

namespace xrc
{
    public interface IKernel
    {
		bool Match(IContext context);

		void ProcessRequest(IContext context);

		void Init();
    }
}
