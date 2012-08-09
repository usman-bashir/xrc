using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc.Views
{
    public class XmlView : IView
    {
        public XElement Content
        {
            get;
            set;
        }

        public void RenderRequest(IContext context)
        {
            if (Content == null)
                throw new ArgumentNullException("Content");

            context.Response.ContentType = "application/xml";
            Content.Save(context.Response.Output);
        }
    }
}
