using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace DemoWebSite
{
    public interface IContactModule : xrc.Modules.IModule
    {
        string SayHello();

        Contact GetDefault();

        void Add(Contact contact);

        Contact[] GetContacts();
    }

    public class ContactModule : IContactModule
	{
        private static ConcurrentBag<Contact> _contacts = new ConcurrentBag<Contact>();

        public string SayHello()
        {
            return "Hello from a module";
        }

        public Contact GetDefault()
		{
			return new Contact() { FirstName = "John", LastName = "Wayne", Message = "Your message" };
		}

        public void Add(Contact contact)
        {
            _contacts.Add(contact);
        }

        public Contact[] GetContacts()
        {
            return _contacts.ToArray();
        }
	}
}