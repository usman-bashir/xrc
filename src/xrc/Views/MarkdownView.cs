using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web.UI;
using System.Xml;
using xrc.Markdown;

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

		public void Execute(IContext context)
        {
            context.Response.ContentType = "text/html";

			if (Content != null)
			{
				string html = _markdown.Transform(Content);
				context.Response.Write(html);
			}
        }
    }
}
