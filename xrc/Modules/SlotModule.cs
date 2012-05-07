using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    public class SlotModule : IModule
    {
        private IContext _context;
        public SlotModule(IContext context)
        {
            _context = context;
        }

		public string Include()
        {
			return Include(string.Empty);
        }

		public string Include(string slotName)
        {
            if (_context.SlotCallback != null)
            {
                RenderSlotEventArgs e = new RenderSlotEventArgs(slotName);
                _context.SlotCallback(this, e);
                return e.Content;
            }
            else
                return string.Empty;
        }
    }
}
