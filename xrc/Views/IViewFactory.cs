using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public interface IViewFactory
    {
        IView Get(ComponentDefinition component, IContext context = null);
        void Release(IView component);
    }
}
