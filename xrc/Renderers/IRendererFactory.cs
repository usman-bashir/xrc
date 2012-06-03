using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Renderers
{
    public interface IRendererFactory
    {
        IRenderer Get(ComponentDefinition component, IContext context = null);
        void Release(IRenderer component);
    }
}
