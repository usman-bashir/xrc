using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml.Linq;
using xrc.Pages.Providers;

namespace xrc.Modules
{
    public class FileModule : IFileModule
    {
        IContext _context;
		IPageProviderService _pageProvider;

		public FileModule(IContext context, IPageProviderService pageProvider)
        {
            _context = context;
			_pageProvider = pageProvider;
        }

		public string VirtualPath(string file)
		{
			return _context.Page.GetResourceLocation(file);
		}

        public XDocument Xml(string file)
        {
			return _pageProvider.ResourceToXml(VirtualPath(file));
        }

		public XDocument XHtml(string file)
		{
			return _pageProvider.ResourceToXHtml(VirtualPath(file));
		}

		public string Html(string file)
		{
			return _pageProvider.ResourceToHtml(VirtualPath(file));
		}

		public string Text(string file)
		{
			return _pageProvider.ResourceToText(VirtualPath(file));
		}

		public byte[] Bytes(string file)
		{
			return _pageProvider.ResourceToBytes(VirtualPath(file));
		}
	}
}
