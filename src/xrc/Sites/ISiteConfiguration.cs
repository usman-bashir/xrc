using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Sites
{
    public interface ISiteConfiguration
    {
        string Key { get; }
        string UriPattern { get; }
        IDictionary<string, string> Parameters { get; }
		bool MatchUrl(Uri url);
	}
}
