using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public interface IModuleFactory
    {
        IModule Get(ComponentDefinition component, IContext context = null);
        void Release(IModule component);
    }
}
