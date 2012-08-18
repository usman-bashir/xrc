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

        Uri GetRelativeUrl(Uri absoluteUrl);

        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path.
        /// If the specified content path does not start with the tilde (~) character, this method returns an absolute url starting from contextUrl.
        /// </summary>
		/// <param name="contextUrl">Is the current context url. Used when the specified uri is a relative url but without tilde.</param>
        string GetAbsoluteUrl(string url, Uri contextUrl);
    }
}
