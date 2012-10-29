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
    public class HtmlView : IView
    {
		readonly IResourceProviderService _resourceProvider;

		public HtmlView(IResourceProviderService resourceProvider)
		{
			_resourceProvider = resourceProvider;
		}

        public string Content
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
				Content = _resourceProvider.ResourceToHtml(context.Page.GetResourceLocation(ContentFile));

            context.Response.ContentType = "text/html";

			if (Content != null)
				context.Response.Write(Content);
        }
    }
}
