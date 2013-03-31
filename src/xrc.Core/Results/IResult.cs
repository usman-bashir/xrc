using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xrc
{
    public interface IResult
    {
        string ContentType { get; }

        void Execute(IContext context);
    }
}
