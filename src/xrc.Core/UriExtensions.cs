using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace xrc
{
	// TODO Valutare se creare una classe Uri2 con tutte le funzionalità di Uri + UriExtensions 

	/// <summary>
	/// A Uri extension utility class. Can work with absolute, relative or virtual url.
	/// </summary>
    public static class UriExtensions
    {
        public static Uri Combine(this Uri baseUri, Uri uri)
        {
            return Combine(baseUri, uri.ToString());
        }

        public static Uri Combine(this Uri baseUri, string uri)
        {
			if (baseUri == null)
				throw new ArgumentNullException("baseUri");
			if (uri == null)
				return baseUri;

			if (!baseUri.IsAbsoluteUri)
			{
				// For relative uri I must create a fake absolute uri because relative uri doesn't support url navigation (..).
				Uri fakeBaseUri = new Uri(new Uri("http://baseuri.fake/"), baseUri);
				Uri combinedUri = Combine(fakeBaseUri, uri);
				string originalStartingUrl = baseUri.ToString()[0] == '/' ? "/" : "";
				string fixedUri = combinedUri.ToString().Replace("http://baseuri.fake/", originalStartingUrl);
				return new Uri(fixedUri, UriKind.Relative);
			}
			else
				return new Uri(baseUri.AppendTrailingSlash(), UriExtensions.RemoveHeadSlash(uri));
        }

        public static string Combine(string baseUri, string uri)
        {
			return Combine(new Uri(baseUri, UriKind.RelativeOrAbsolute), uri).ToString();
        }

        public static bool IsBaseOfWithPath(this Uri baseUri, Uri uri)
        {
            // Note: Uri.IsBaseOf returns true also when called with
            // new Uri("http://contoso.com/path").IsBaseOf(new Uri("http://contoso.com"))
            // Which for me is wrong...so I check also the path

			if (string.IsNullOrWhiteSpace(baseUri.ToString()))
				return false;

			if (baseUri.IsAbsoluteUri && uri.IsAbsoluteUri)
			{
				baseUri = baseUri.ToLower();
				uri = uri.ToLower();

				if (baseUri.Scheme == uri.Scheme &&
					baseUri.Host == uri.Host &&
					baseUri.Port == uri.Port &&
					uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped).StartsWith(baseUri.GetComponents(UriComponents.Path, UriFormat.UriEscaped).Trim('/')))
					return true;
				else
					return false;
			}
			else if (!baseUri.IsAbsoluteUri && !uri.IsAbsoluteUri)
			{
				baseUri = baseUri.RemoveTrailingSlash();

				return uri.ToString().StartsWith(baseUri.ToString(), StringComparison.InvariantCultureIgnoreCase);
			}
			else
				throw new XrcException("Uri not valid, cannot compare a relative uri with an absolute uri.");
        }

        public static Uri MakeRelativeUriEx(this Uri uri, Uri baseUri)
        {
            // Note: Uri.MakeRelativeUri doesn't work on this case:
            // new Uri("http://contoso.com/vpath").MakeRelativeUri(new Uri("http://contoso.com/vpath/"))
            // returns ../vpath but I expect it returns ''

            if (!baseUri.IsBaseOfWithPath(uri))
                return uri;

			if (baseUri.IsAbsoluteUri && uri.IsAbsoluteUri)
			{
				string basePath = baseUri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped).Trim('/');
				string uriPath = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped).TrimStart('/');

				string uriv = uriPath;
				if (basePath.Length != 0)
					uriv = Regex.Replace(uriv, basePath, "", RegexOptions.IgnoreCase).TrimStart('/');
				uriv = uriv.TrimStart('/');
				return new Uri(uriv, UriKind.RelativeOrAbsolute);
			}
			else if (!baseUri.IsAbsoluteUri && !uri.IsAbsoluteUri)
			{
				string baseUriValid = baseUri.RemoveTrailingSlash().RemoveHeadSlash().ToString();
				string urlString = uri.RemoveHeadSlash().ToString();
				if (urlString.StartsWith(baseUriValid, StringComparison.InvariantCultureIgnoreCase))
				{
					urlString = urlString.Substring(baseUriValid.Length);
					urlString = RemoveHeadSlash(urlString);
				}

				return new Uri(urlString, UriKind.Relative);
			}
			else
				throw new XrcException("Uri not valid, cannot compare a relative uri with an absolute uri.");
        }

        public static Uri ToLower(this Uri uri)
        {
            string uriv = uri.ToString().ToLowerInvariant();
            return new Uri(uriv, UriKind.RelativeOrAbsolute);
        }

		public static Uri RemoveHeadSlash(this Uri uri)
		{
			string uriv = RemoveHeadSlash(uri.ToString());
			return new Uri(uriv, UriKind.RelativeOrAbsolute);
		}

		public static string RemoveHeadSlash(string uri)
		{
			return uri.TrimStart('/');
		}

		/// <summary>
		/// Appends the literal slash mark (/) to the end of the virtual path, if one does not already exist.
		/// </summary>
		public static Uri AppendTrailingSlash(this Uri uri)
        {
			string uriv = AppendTrailingSlash(uri.ToString());
            return new Uri(uriv, UriKind.RelativeOrAbsolute);
        }

		/// <summary>
		/// Appends the literal slash mark (/) to the end of the virtual path, if one does not already exist.
		/// </summary>
		public static string AppendTrailingSlash(string uri)
		{
			if (uri == null)
				return "/";

			// Base uri must end with a slash otherwise are considered as files
			if (!uri.EndsWith("/"))
				uri += "/";

			return uri;
		}

		public static Uri RemoveTrailingSlash(this Uri uri)
		{
			string uriv = RemoveTrailingSlash(uri.ToString());
			return new Uri(uriv, UriKind.RelativeOrAbsolute);
		}

		public static string RemoveTrailingSlash(string uri)
		{
			if (uri == null)
				return null;

			return uri.TrimEnd('/');
		}

        public static Uri ToSecure(this Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = 443;
            return builder.Uri;
        }

		/// <summary>
		/// Similar to Url.GetComponents(UriComponents.Path, UriFormat.Unescaped) but support also relative uri
		/// </summary>
		public static string GetPath(this Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			if (uri.IsAbsoluteUri)
				return Combine("/", uri.GetComponents(UriComponents.Path, UriFormat.Unescaped));
			else
				return uri.ToString().Split('?', '#')[0];
		}

		/// <summary>
		/// Similar to Url.GetComponents(UriComponents.Path) but support also relative uri
		/// </summary>
		public static string GetPath(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			return GetPath(new Uri(uri, UriKind.RelativeOrAbsolute));
		}

        public static string GetName(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            return GetName(uri.ToString());
        }

        public static string GetName(string uri)
        {
			if (uri == null)
				throw new ArgumentNullException("uri");

            var parts = uri.Split('/', '\\');

            return parts.Last();
        }

		/// <summary>
		/// Similar to Url.GetComponents(UriComponents.Query, UriFormat.Unescaped) but support also relative uri
		/// </summary>
		public static string GetQuery(this Uri uri)
		{
			return GetQuery(uri.ToString());
		}

		static Regex QueryExtractRegEx = new Regex(@".*(?<query>\?.*)$", RegexOptions.Compiled);
		static Regex QueryAnchorExtractRegEx = new Regex(@".*(?<query>#.*)$", RegexOptions.Compiled);
		/// <summary>
		/// Similar to Url.GetComponents(UriComponents.Query) but support also relative uri
		/// </summary>
		public static string GetQuery(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			var match = QueryExtractRegEx.Match(uri);

			if (match.Success)
				return match.Groups["query"].Value.TrimStart('?');
			else
			{
				match = QueryAnchorExtractRegEx.Match(uri);

				if (match.Success)
					return match.Groups["query"].Value.TrimStart('?');
				else
					return string.Empty;
			}
		}

		public static string BuildVirtualPath(string contextVirtualPath, string virtualPath)
		{
			if (VirtualPathUtility.IsAbsolute(virtualPath))
				return virtualPath;

			return VirtualPathUtility.Combine(contextVirtualPath, virtualPath);
		}

		public static string AppRelativeUrlToRelativeUrl(string virtualPath, string applicationPath)
		{
			if (VirtualPathUtility.IsAppRelative(virtualPath))
				return VirtualPathUtility.ToAbsolute(virtualPath, Combine("/", applicationPath));
			else if (VirtualPathUtility.IsAbsolute(virtualPath))
				return virtualPath;
			else
				return Combine(applicationPath, virtualPath);
		}

		public static string RelativeUrlToAppRelativeUrl(string url, Uri applicationPath)
		{
			if (VirtualPathUtility.IsAppRelative(url))
				return url;
			else
				return UriExtensions.Combine("~/", UriExtensions.MakeRelativeUriEx(new Uri(url, UriKind.RelativeOrAbsolute), applicationPath).ToString());
		}

		public static bool IsAppRelativeVirtualUrl(string appRelativeUrl)
		{
			return VirtualPathUtility.IsAppRelative(appRelativeUrl);
		}
	}
}
