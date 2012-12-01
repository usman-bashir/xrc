using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public class WindsorViewFactory : IViewFactory
    {
        private Castle.MicroKernel.IKernel _windsorKernel;

		public WindsorViewFactory(Castle.MicroKernel.IKernel windsorKernel)
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
