using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public interface IModuleCatalogService
    {
        IEnumerable<ComponentDefinition> GetAll();
        ComponentDefinition Get(string name);
		bool TryGet(string name, out ComponentDefinition component);
    }
}
