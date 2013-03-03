using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
	public class PageRoot : PageDirectory
    {
        public PageRoot(string rootLocation, params Item[] items)
            : base(rootLocation, "~", items)
        {
        }
    }
}
