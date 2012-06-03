using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Renderers;

namespace xrc.IoC.Windsor
{
    public class RendererFactory : IRendererFactory
    {
        private Castle.MicroKernel.IKernel _windsorKernel;

        public RendererFactory(Castle.MicroKernel.IKernel windsorKernel)
        {
            _windsorKernel = windsorKernel;
        }

        public IRenderer Get(ComponentDefinition component, IContext context = null)
        {
            return (IRenderer)_windsorKernel.Resolve(component.Type, new { context = context });
        }

        public void Release(IRenderer component)
        {
            _windsorKernel.ReleaseComponent(component);
        }
    }
}
