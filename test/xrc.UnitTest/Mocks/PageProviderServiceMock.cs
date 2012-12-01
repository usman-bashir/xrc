using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;
using System.Linq.Expressions;
using xrc.Pages.Providers;

namespace xrc.Mocks
{
    class PageProviderServiceMock : IPageProviderService
    {
		public bool PageExists(Uri url)
		{
			throw new NotImplementedException();
		}

		public System.IO.Stream OpenResource(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public System.Xml.Linq.XDocument ResourceToXml(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public System.Xml.Linq.XDocument ResourceToXHtml(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public string ResourceToHtml(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public string ResourceToText(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public byte[] ResourceToBytes(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public bool ResourceExists(string resourceLocation)
		{
			throw new NotImplementedException();
		}

		public bool PageExists(XrcUrl url)
		{
			throw new NotImplementedException();
		}

		public Pages.IPage GetPage(XrcUrl url)
		{
			throw new NotImplementedException();
		}
	}
}
