using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.TreeStructure
{
    public interface IPageLocatorService
    {
		PageLocatorResult Locate(XrcUrl url);
    }
}
