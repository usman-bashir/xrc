using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
	// TODO Valutare se creare una classe Uri2 con tutte le funzionalità di Uri + UriExtensions 

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

        public static Uri MakeRelativeUriEx(this Uri uri, Uri baseUri)
        {
            // Note: Uri.MakeRelativeUri doesn't work on this case:
            // new Uri("http://contoso.com/vpath").MakeRelativeUriEx(new Uri("http://contoso.com/vpath/"))
            // returns ../vpath but I expect it returns ''

            if (!baseUri.IsBaseOfWithPath(uri))
                return uri;

            string basePath = baseUri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped).Trim('/').ToLower();
            string uriPath = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped).TrimStart('/').ToLower();

            string uriv = uriPath;
            if (basePath.Length != 0)
                uriv = uriv.Replace(basePath, "").TrimStart('/');
            uriv = uriv.TrimStart('/');
            return new Uri(uriv, UriKind.RelativeOrAbsolute);
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
		/// Similar to Url.GetLeftPart(UriPartial.Path) but support also relative uri
		///  and doesn't encode the results		
		/// </summary>
		public static string GetPath(this Uri uri)
		{
			return GetPath(uri.ToString());
		}

		/// <summary>
		/// Similar to Url.GetLeftPart(UriPartial.Path) but support also relative uri
		///  and doesn't encode the results		
		/// </summary>
		public static string GetPath(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			// Similar to Url.GetLeftPart(UriPartial.Path) but support also relative uri
			//  and doesn't encode the results
			return uri.Split('?', '#')[0];
		}

		public static string BuildVirtualPath(string contextVirtualPath, string virtualPath)
		{
			if (VirtualPathUtility.IsAbsolute(virtualPath))
				return virtualPath;

			return VirtualPathUtility.Combine(contextVirtualPath, virtualPath);
		}
    }
}
