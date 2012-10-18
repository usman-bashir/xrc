using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common
{
    public interface IPageParserService
    {
		PageParserResult Parse(XrcItem item);
    }
}
