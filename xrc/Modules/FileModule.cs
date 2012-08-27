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

        public XDocument Xml(string file)
        {
			XDocument doc;

			using (Stream stream = _pageProvider.GetPageResource(_context.Page, file))
			{
				doc = XDocument.Load(stream);
			}

			return doc;
        }

		public XDocument XHtml(string file)
		{
			return Xml(file);
		}
	}
}
