using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public class ModuleCatalogService : ComponentCatalogService<IModule>, IModuleCatalogService
    {
        public ModuleCatalogService(Configuration.IModuleConfig config)
			: base(config.Modules)
        {

        }
    }
}
