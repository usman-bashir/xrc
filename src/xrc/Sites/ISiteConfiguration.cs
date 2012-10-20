using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Sites
{
    public interface ISiteConfiguration
    {
        string Key { get; }
        Uri Uri { get; }
        Uri SecureUri { get; }
        IDictionary<string, string> Parameters { get; }
	}
}
