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
        ContentResult Page(Uri url, object parameters = null, IContext callerContext = null);
    }
}
