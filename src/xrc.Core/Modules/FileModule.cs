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
        readonly IContext _context;
		readonly IResourceProviderService _resourceProvider;

		public FileModule(IContext context, IResourceProviderService resourceProvider)
        {
            _context = context;
			_resourceProvider = resourceProvider;
        }

		public string VirtualPath(string file)
		{
			return _context.Page.GetResourceLocation(file);
		}

        public XDocument Xml(string file)
        {
			return _resourceProvider.ResourceToXml(VirtualPath(file));
        }

		public XDocument XHtml(string file)
		{
			return _resourceProvider.ResourceToXHtml(VirtualPath(file));
		}

		public string Html(string file)
		{
			return _resourceProvider.ResourceToHtml(VirtualPath(file));
		}

		public string Text(string file)
		{
			return _resourceProvider.ResourceToText(VirtualPath(file));
		}

		public byte[] Bytes(string file)
		{
			return _resourceProvider.ResourceToBytes(VirtualPath(file));
		}
	}
}
