using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web.UI;
using System.Xml;
using xrc.Pages.Providers;

namespace xrc.Views
{
    public class XHtmlView : IView
    {
		readonly IResourceProviderService _resourceProvider;

		public XHtmlView(IResourceProviderService resourceProvider)
		{
			_resourceProvider = resourceProvider;
		}

        public XDocument Content
        {
            get;
            set;
        }

		public string ContentFile
		{
			get;
			set;
		}

		public void Execute(IContext context)
        {
			if (Content == null && !string.IsNullOrEmpty(ContentFile))
				Content = _resourceProvider.ResourceToXml(context.Page.GetResourceLocation(ContentFile));

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
