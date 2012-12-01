using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc.Pages.Providers
{
	public interface IPageProviderService
	{
		bool PageExists(XrcUrl url);

		IPage GetPage(XrcUrl url);
	}
}
