using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Web.AcceptanceTest
{
	public class TestView : xrc.Views.IView
	{
		public void Execute(xrc.IContext context)
		{
			context.Response.Write("<h3>TestView View</h3>");
			context.Response.Write(string.Format("<p>Hello from TestView view! {0}</p>", Param1));
		}

		public string Param1
		{
			get;
			set;
		}
	}
}
