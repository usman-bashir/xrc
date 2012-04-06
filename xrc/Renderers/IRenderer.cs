using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Renderers
{
    public interface IRenderer
    {
		void RenderRequest(IContext context);
    }
}
