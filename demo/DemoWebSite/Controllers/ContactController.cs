using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebSite.Controllers
{
    public class ContactController : Controller
    {
        private IContactModule _contactModule;
        private xrc.IXrcService _xrc;
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

            // TODO Valutare se utilizzare un metodo migliore per ottenere un url assoluto o altrimenti prevedere di passare anche un url relativo. Valutare anche il ToLower se necessario.
            Uri absoluteUrl = new Uri(Request.Url, VirtualPathUtility.ToAbsolute("~/contact/_sendsuccess"));
			absoluteUrl = xrc.UriExtensions.ToLower(absoluteUrl);

			return _xrc.Page(absoluteUrl, new { newContact = newContact });
        }
    }
}
