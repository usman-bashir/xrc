using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Views;

namespace xrc.IoC.Windsor
{
    public class ViewFactory : IViewFactory
    {
        private Castle.MicroKernel.IKernel _windsorKernel;

        public ViewFactory(Castle.MicroKernel.IKernel windsorKernel)
        {
            _windsorKernel = windsorKernel;
        }

        public IView Get(ComponentDefinition component, IContext context = null)
        {
            return (IView)_windsorKernel.Resolve(component.Type, new { context = context });
        }

        public void Release(IView component)
        {
            _windsorKernel.ReleaseComponent(component);
        }
    }
}
