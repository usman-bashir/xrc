using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.CustomErrors
{
    public interface ICustomErrorEntry
    {
        int StatusCode { get; }
        string Url { get; }
	}
}
