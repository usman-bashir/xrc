using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebSite.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactModule _contactModule;
		readonly xrc.IXrcService _xrc;
		readonly xrc.Sites.ISiteConfigurationProviderService _siteConfigurationProvider;

        public ContactController(IContactModule contactModule,
                                xrc.IXrcService xrc,
								xrc.Sites.ISiteConfigurationProviderService siteConfigurationProvider)
        {
            _contactModule = contactModule;
            _xrc = xrc;
			_siteConfigurationProvider = siteConfigurationProvider;
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

			return _xrc.Page(new xrc.XrcUrl("~/contact/_sendsuccess"), GetSiteConfiguration(), new { newContact = newContact });
        }

		xrc.Sites.ISiteConfiguration GetSiteConfiguration()
		{
			return _siteConfigurationProvider.GetSiteFromUri(Request.Url);
		}
    }
}
