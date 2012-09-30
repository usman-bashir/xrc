using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web.UI;
using System.Xml;
using xrc.Markdown;
using System.Web;

namespace xrc.Views
{
	public class MarkdownView : IView
    {
		readonly IMarkdownService _markdown;

		public MarkdownView(IMarkdownService markdown)
		{
			_markdown = markdown;
		}

        public string Content
        {
            get;
            set;
        }

		public string BaseUrl
		{
			get;
			set;
		}

		public void Execute(IContext context)
        {
            context.Response.ContentType = "text/html";

			string currentBaseUrl;
			if (BaseUrl != null)
				currentBaseUrl = BaseUrl;
			else
			{
				if (HttpRuntime.AppDomainAppVirtualPath != null)
					currentBaseUrl = HttpRuntime.AppDomainAppVirtualPath;
				else
					currentBaseUrl = null;
			}

			if (Content != null)
			{
				string html = _markdown.Transform(Content, currentBaseUrl);
				context.Response.Write(html);
			}
        }
    }
}
