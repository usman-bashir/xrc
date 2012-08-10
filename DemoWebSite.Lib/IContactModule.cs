using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoWebSite
{
	public interface IContactModule : xrc.Modules.IModule
	{
		string SayHello();

		Contact GetDefault();

		void Add(Contact contact);

		ContactList GetContacts();
	}
}
