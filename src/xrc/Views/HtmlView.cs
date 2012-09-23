using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web.UI;
using System.Xml;

namespace xrc.Views
{
    public class HtmlView : IView
    {
        public string Content
        {
            get;
            set;
        }

		public void Execute(IContext context)
        {
            context.Response.ContentType = "text/html";

			if (Content != null)
				context.Response.Write(Content);
        }
    }
}
