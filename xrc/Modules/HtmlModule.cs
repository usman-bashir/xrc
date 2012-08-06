using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Net;

namespace xrc.Modules
{
    public class HtmlModule : IHtmlModule
    {
        private IContext _context;
        private IKernel _kernel;
        public HtmlModule(IContext context, IKernel kernel)
        {
            _context = context;
            _kernel = kernel;
        }

    }
}
