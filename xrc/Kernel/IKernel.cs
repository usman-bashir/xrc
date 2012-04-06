using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    public interface IKernel : IRootPath
    {
        void RenderRequest(IContext context);

        List<Module> Modules
		{
			get;
		}
    }

    public interface IRootPath
    {
        string Path { get; }
    }
}
