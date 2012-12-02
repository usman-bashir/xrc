using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xrc.Web.AcceptanceTest
{
    public class Contact
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string Message { get; set; }
	}

	public class ContactList
	{
		public ContactList(Contact[] contacts)
		{
			Contacts = contacts;
		}

		public Contact[] Contacts { get; set; }
	}
}