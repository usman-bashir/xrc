﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc.Pages.Providers
{
	public interface IResourceProviderService
	{
		/// <summary>
		/// Open the specified resource as a stream.
		/// </summary>
        /// <param name="resourceLocation">Can be a fully qualified resource or a virtual path.</param>
		System.IO.Stream OpenResource(string resourceLocation);

		XDocument ResourceToXml(string resourceLocation);
		XDocument ResourceToXHtml(string resourceLocation);
		string ResourceToHtml(string resourceLocation);
		string ResourceToText(string resourceLocation);
		byte[] ResourceToBytes(string resourceLocation);
		string ResourceToJson(string resourceLocation);

		bool ResourceExists(string resourceLocation);
	}
}
