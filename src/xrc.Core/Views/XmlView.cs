using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Pages.Providers;

namespace xrc.Views
{
    public class XmlView : IView
    {
		readonly IResourceProviderService _resourceProvider;

		public XmlView(IResourceProviderService resourceProvider)
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

            context.Response.ContentType = "application/xml";
            Content.Save(context.Response.Output);
        }
    }
}
