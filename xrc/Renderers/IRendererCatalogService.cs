using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Renderers
{
    public interface IRendererCatalogService
    {
        IEnumerable<ComponentDefinition> GetAll();
        ComponentDefinition Get(string name);
    }
}
