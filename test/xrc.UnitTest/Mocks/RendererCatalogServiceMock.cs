using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;

namespace xrc.Mocks
{
    class ViewCatalogServiceMock : IViewCatalogService
    {
        ComponentDefinition _component;


		public ViewCatalogServiceMock()
		{
		}

        public ViewCatalogServiceMock(ComponentDefinition component)
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
