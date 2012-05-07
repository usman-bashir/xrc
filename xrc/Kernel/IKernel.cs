using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    public interface IKernel
    {
        void RenderRequest(IContext context);
    }
}
