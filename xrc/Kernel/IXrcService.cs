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
        ContentResult Page(string url, object parameters = null, HttpRequestBase parentRequest = null, HttpResponseBase parentResponse = null);
    }
}
