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

		Uri AbsoluteUrlToRelative(Uri absoluteUrl);
		string RelativeUrlToVirtual(Uri relativeUrl);
		Uri VirtualUrlToRelative(string virtualUrl);
		Uri VirtualUrlToAbsolute(string virtualUrl);

		// da rimuovere
        Uri ToRelativeUrl(Uri absoluteUrl);
		Uri ToAbsoluteUrl(string virtualUrl, Uri contextUrl);
	}
}
