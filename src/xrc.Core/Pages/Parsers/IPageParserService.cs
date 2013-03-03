using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Parsers
{
    public interface IPageParserService
    {
		PageDefinition Parse(string resourceLocation);
    }
}
