using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Renderers
{
    public class RendererCatalogService : ComponentCatalogService<IRenderer>, IRendererCatalogService
    {
        public RendererCatalogService(Configuration.XrcSection configuration)
            :base(configuration.Renderers)
        {

        }
    }
}
