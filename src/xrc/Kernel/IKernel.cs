using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
    public interface IKernel
    {
		bool Match(HttpContextBase httpContext);

		void ProcessRequest(HttpContextBase httpContext);

		void Init();
    }
}
