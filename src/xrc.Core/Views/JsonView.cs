using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Providers;

namespace xrc.Views
{
    public class JsonView : IView
    {
		readonly IResourceProviderService _resourceProvider;

		public JsonView(IResourceProviderService resourceProvider)
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
			if (Content == null)
				throw new ArgumentNullException("Content");

			if (Content == null && !string.IsNullOrEmpty(ContentFile))
				Content = _resourceProvider.ResourceToJson(context.Page.GetResourceLocation(ContentFile));

            context.Response.ContentType = "application/json";
			context.Response.Output.Write(Content.ToString());
        }
    }
}
