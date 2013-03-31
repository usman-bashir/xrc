using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xrc.Web.AcceptanceTest.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactModule _contactModule;
		readonly xrc.IXrcService _xrc;

        public ContactController(IContactModule contactModule,
                                xrc.IXrcService xrc)
        {
            _contactModule = contactModule;
            _xrc = xrc;
        }

        public ActionResult SendMVC(string firstName, string lastName, string message)
        {
            Contact newContact = new Contact() { FirstName = firstName, LastName = lastName, Message = message };
            _contactModule.Add(newContact);

            return View(newContact);
        }

        public ActionResult SendXRC(string firstName, string lastName, string message)
        {
            Contact newContact = new Contact() { FirstName = firstName, LastName = lastName, Message = message };
            _contactModule.Add(newContact);

            var xrcResult = _xrc.Page(new xrc.XrcUrl("~/razor/_sendsuccess"), new { newContact = newContact });
			return xrcResult.AsActionResult();
        }
    }
}
