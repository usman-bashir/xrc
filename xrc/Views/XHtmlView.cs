using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web.UI;
using System.Xml;

namespace xrc.Views
{
    public class XHtmlView : IView
    {
        public XDocument Content
        {
            get;
            set;
        }

		public void Execute(IContext context)
        {
            if (Content == null)
                throw new ArgumentNullException("Content");

            context.Response.ContentType = "text/html";

			//using (XhtmlTextWriter htmlOutput = new XhtmlTextWriter(context.Response.Output))
			//{
			//    Content.Root.Save(htmlOutput);
			//}

			XmlWriterSettings xws = new XmlWriterSettings();
			if (Content.Declaration == null)
				xws.OmitXmlDeclaration = true;
			using (XmlWriter htmlOutput = XmlWriter.Create(context.Response.Output, xws))
			{
				Content.Save(htmlOutput);
			}
        }
    }
}
