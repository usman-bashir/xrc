using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers
{
	public interface IPageProviderService
	{
		IPage GetPage(Uri url);

		System.IO.Stream GetPageResource(IPage page, string resourceLocation);

		bool IsDefined(Uri url);
	}
}
