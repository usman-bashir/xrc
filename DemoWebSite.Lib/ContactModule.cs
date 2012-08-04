using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebSite
{
    public interface IContactModule : xrc.Modules.IModule
    {
        string SayHello();

        Contact GetDefault();
    }

    public class ContactModule : IContactModule
	{
        public string SayHello()
        {
            return "Hello from a module";
        }

        public Contact GetDefault()
		{
			return new Contact() { FirstName = "John", LastName = "Wayne", Message = "Your message" };
		}
	}
}