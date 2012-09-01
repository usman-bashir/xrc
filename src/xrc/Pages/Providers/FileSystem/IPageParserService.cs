using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.FileSystem
{
    public interface IPageParserService
    {
		PageParserResult Parse(XrcFile file);
    }
}
