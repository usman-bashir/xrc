using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc.Markdown
{
	public class MarkdownService : IMarkdownService
	{
		public string Transform(string markdownText)
		{
			// TODO Valutare se usare un'altra libreria per performance migliori e perchè credo che questa non supporta il multithreading e  bisogna ricreare ogni volta l'istanza. Da verificare.

			var markdown = new MarkdownSharp.Markdown();
			if (HttpRuntime.AppDomainAppVirtualPath != null)
				markdown.BaseUrl = HttpRuntime.AppDomainAppVirtualPath;
			else
				markdown.BaseUrl = "";

			return markdown.Transform(markdownText);
		}
	}
}
