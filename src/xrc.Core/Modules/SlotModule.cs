using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Web;

namespace xrc.Modules
{
    public class SlotModule : ISlotModule
    {
        private IContext _context;
        private IXrcService _xrcService;
        public SlotModule(IContext context, IXrcService xrcService)
        {
            _context = context;
            _xrcService = xrcService;
        }

        public string IncludeChild()
        {
            return IncludeChild(string.Empty);
        }

        public string IncludeChild(string slotName)
        {
            if (_context.SlotCallback != null)
            {
                RenderSlotEventArgs e = new RenderSlotEventArgs(slotName);
                _context.SlotCallback(this, e);
                return e.Result.Content;
            }
            else
                return string.Empty;
        }

        public string Include(string url)
        {
            return Include(url, null);
        }

        public string Include(string url, object parameters)
        {
			XrcUrl xrcUrl = _context.Page.GetPageUrl(url);

			return _xrcService.Page(xrcUrl, parameters, callerContext: _context).Content;
        }
    }
}
