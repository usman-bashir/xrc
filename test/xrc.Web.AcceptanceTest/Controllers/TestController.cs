using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xrc.Web.AcceptanceTest.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
		public ActionResult PartialAction()
        {
            return View();
        }
    }
}
