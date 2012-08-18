using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public class ViewCatalogService : ComponentCatalogService<IView>, IViewCatalogService
    {
        public ViewCatalogService(Configuration.IViewConfig config)
			: base(config.Views)
        {

        }
    }
}
