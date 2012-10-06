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
			return UriExtensions.BuildVirtualPath(_context.Page.VirtualPath, file);
		}

        public XDocument Xml(string file)
        {
			XDocument doc;

			using (Stream stream = _pageProvider.OpenResource(VirtualPath(file)))
			{
				doc = XDocument.Load(stream);
			}

			return doc;
        }

		public XDocument XHtml(string file)
		{
			return Xml(file);
		}


		public string Html(string file)
		{
			return Text(file);
		}

		public string Text(string file)
		{
			using (StreamReader stream = new StreamReader(_pageProvider.OpenResource(VirtualPath(file)), true))
			{
				string text = stream.ReadToEnd();
				stream.Close();

				return text;
			}
		}
	}
}
