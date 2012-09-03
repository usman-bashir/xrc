using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.IoC.Windsor
{
    public class ModuleFactory : IModuleFactory
    {
        private Castle.MicroKernel.IKernel _windsorKernel;

        public ModuleFactory(Castle.MicroKernel.IKernel windsorKernel)
        {
            _windsorKernel = windsorKernel;
        }

        public object Get(ComponentDefinition component, IContext context = null)
        {
            return _windsorKernel.Resolve(component.Type, new { context = context });
        }

		public void Release(object component)
        {
            _windsorKernel.ReleaseComponent(component);
        }
    }
}
