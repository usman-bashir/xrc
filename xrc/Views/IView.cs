using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public interface IView
    {
		void RenderRequest(IContext context);
    }
}
