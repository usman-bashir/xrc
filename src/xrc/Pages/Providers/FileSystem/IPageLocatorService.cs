using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.FileSystem
{
    public interface IPageLocatorService
    {
        XrcFile Locate(Uri relativeUri);
    }
}
