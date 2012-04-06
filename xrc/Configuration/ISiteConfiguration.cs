using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public interface ISiteConfiguration
    {
        string Key { get; }
        Uri Uri { get; }
        Uri SecureUri { get; }
        IDictionary<string, string> Parameters { get; }
        Uri GetRelativeUri(Uri absoluteUri);

        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path.
        /// If the specified content path does not start with the tilde (~) character, this method returns contentPath unchanged.
        /// </summary>
		/// <param name="requestContext">Is the current context url. Used when the specified uri is a relative url but without tilde.</param>
        string UrlContent(string contentUri, Uri contextUri);
    }
}
