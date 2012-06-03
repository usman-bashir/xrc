using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Renderers;

namespace xrc.Mocks
{
    class RendererCatalogServiceMock : IRendererCatalogService
    {
        ComponentDefinition _component;

        public RendererCatalogServiceMock(ComponentDefinition component)
        {
            _component = component;
        }

        public IEnumerable<ComponentDefinition> GetAll()
        {
            throw new NotImplementedException();
        }

        public ComponentDefinition Get(string name)
        {
            return _component;
        }
    }
}
