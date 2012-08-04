using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml.Linq;

namespace xrc.Modules
{
    public class FileModule : IFileModule
    {
        private IContext _context;
        public FileModule(IContext context)
        {
            _context = context;
        }

        public XDocument Xml(string file)
        {
            string fullPath = Path.Combine(_context.WorkingPath, file);

			XDocument doc = XDocument.Load(fullPath);

			return doc;
        }

		public XDocument XHtml(string file)
		{
			return Xml(file);
		}
	}
}
