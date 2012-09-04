using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers
{
	public interface IPageProviderService
	{
		bool IsDefined(Uri url);

		IPage GetPage(Uri url);

		System.IO.Stream OpenPageResource(IPage page, string resourceLocation);

		string GetPageVirtualPath(IPage page, string url);
	}
}
