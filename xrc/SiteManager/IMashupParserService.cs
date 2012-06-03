using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public interface IMashupParserService
    {
        MashupPage Parse(string file);
    }
}
