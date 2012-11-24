using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace xrc
{
    public interface IXrcService
    {
		bool Match(XrcUrl url);

		void ProcessRequest(IContext context, Sites.ISiteConfiguration siteConfiguration);

		ContentResult Page(XrcUrl url, Sites.ISiteConfiguration siteConfiguration, object parameters = null, IContext callerContext = null);
    }
}
