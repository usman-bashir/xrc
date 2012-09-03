using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Mocks
{
    class ModuleCatalogServiceMock : IModuleCatalogService
    {
        ComponentDefinition _component;

        public ModuleCatalogServiceMock(ComponentDefinition component)
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

		public bool TryGet(string name, out ComponentDefinition component)
		{
			component = _component;
			return true;
		}
	}
}
