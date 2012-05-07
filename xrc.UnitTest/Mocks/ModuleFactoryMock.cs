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
        IModule _module;
        public ModuleFactoryMock(IModule module)
        {
            _module = module;
        }

        public IModule Get(ComponentDefinition component, IContext context = null)
        {
            return _module;
        }

        public void Release(IModule component)
        {
            
        }
    }
}
