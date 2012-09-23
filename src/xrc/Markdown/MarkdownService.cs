using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc.Markdown
{
	public class MarkdownService : IMarkdownService
	{
		readonly MarkdownSharp.Markdown _markdown;

		public MarkdownService()
		{
			_markdown = new MarkdownSharp.Markdown();
			if (HttpRuntime.AppDomainAppVirtualPath != null)
				_markdown.BaseUrl = HttpRuntime.AppDomainAppVirtualPath;
			else
				_markdown.BaseUrl = "";
		}

		public string Transform(string markdownText)
		{
			return _markdown.Transform(markdownText);
		}
	}
}
