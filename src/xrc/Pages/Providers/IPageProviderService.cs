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

		IPage GetPage(XrcUrl url, Sites.ISiteConfiguration siteConfiguration);

		/// <summary>
		/// Open the specified resource. resourceLocation parameter can be a fully qualified resource or a virtual path.
		/// </summary>
		System.IO.Stream OpenResource(string resourceLocation);

		XDocument ResourceToXml(string resourceLocation);
		XDocument ResourceToXHtml(string resourceLocation);
		string ResourceToHtml(string resourceLocation);
		string ResourceToText(string resourceLocation);
		byte[] ResourceToBytes(string resourceLocation);

		bool ResourceExists(string resourceLocation);
	}
}
