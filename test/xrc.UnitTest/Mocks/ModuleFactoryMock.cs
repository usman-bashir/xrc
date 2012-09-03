using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using xrc.Modules;

namespace xrc.Mocks
{
    class ModuleFactoryMock : IModuleFactory
    {
		object _module;
        public ModuleFactoryMock(object module)
        {
            _module = module;
        }

		public object Get(ComponentDefinition component, IContext context)
        {
            return _module;
        }

		public void Release(object component)
        {
            
        }
    }
}
