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

		/// <summary>
		/// Open the specified resource. resourceLocation parameter can be a fully qualified resource or a virtual path.
		/// </summary>
		System.IO.Stream OpenResource(string resourceLocation);
	}
}
