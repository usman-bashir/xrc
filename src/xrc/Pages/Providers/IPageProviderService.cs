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

		/// <summary>
		/// Returns the application virtual path for the specified url using the page as the context.
		/// The path returned is a full application path, with also the xrc folder root.
		/// </summary>
		string GetPageVirtualPath(IPage page, string url);
	}
}
