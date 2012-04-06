using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public interface IMashupLocatorService
    {
        MashupFile Locate(Uri relativeUri);
    }
}
