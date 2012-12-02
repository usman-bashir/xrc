using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Web.AcceptanceTest
{
	public interface IContactModule
	{
		string SayHello();

		Contact GetDefault();

		void Add(Contact contact);

		ContactList GetContacts();
	}
}
