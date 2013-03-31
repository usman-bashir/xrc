using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace xrc
{
    public static class ResultExtensions
    {
        public static ActionResult AsActionResult(this StringResult result)
        {
            return new ContentResult()
            {
                Content = result.Content,
                ContentEncoding = result.ContentEncoding,
                ContentType = result.ContentType
            };
        }
    }
}
