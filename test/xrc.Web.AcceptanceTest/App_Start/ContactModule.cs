using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace xrc.Web.AcceptanceTest
{
    public class ContactModule : IContactModule
	{
        private static ConcurrentBag<Contact> _contacts = new ConcurrentBag<Contact>();

        public string SayHello()
        {
            return "Hello from a module";
        }

        public Contact GetDefault()
		{
			return new Contact() { FirstName = "John", LastName = "Wayne", Message = "Your message for test." };
		}

        public void Add(Contact contact)
        {
            _contacts.Add(contact);
        }

        public ContactList GetContacts()
        {
            return new ContactList(_contacts.ToArray());
        }
	}
}