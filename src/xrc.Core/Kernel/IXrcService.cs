using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
    public interface IXrcService
    {
		bool Match(XrcUrl url);

		void ProcessRequest(IContext context);

		StringResult Page(XrcUrl url, object parameters = null, IContext callerContext = null);
    }
}
