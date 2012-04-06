using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebSite.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello from mvc controller");
        }
    }
}
