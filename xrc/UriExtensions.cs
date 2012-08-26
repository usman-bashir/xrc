using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            string bUri = baseUri.ToString();

            uri = uri.TrimStart('/');
            if (!bUri.EndsWith("/"))
                bUri += "/";

            return new Uri(bUri + uri);
        }

        public static string Combine(string baseUri, string uri)
        {
            string bUri = baseUri.ToString();

            uri = uri.TrimStart('/');
            if (!bUri.EndsWith("/"))
                bUri += "/";

            return bUri + uri;
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

        public static Uri AppendSlash(this Uri uri)
        {
            string uriv = uri.ToString();
            // Base uri must end with a slash otherwise are considered as files
            if (!uriv.EndsWith("/"))
                uriv += "/";

            return new Uri(uriv, UriKind.RelativeOrAbsolute);
        }

        public static Uri ToSecure(this Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = 443;
            return builder.Uri;
        }

		public static string GetPath(this Uri uri)
		{
			// Similar to Url.GetLeftPart(UriPartial.Path) but support also relative uri
			if (uri.IsAbsoluteUri)
				return uri.GetLeftPart(UriPartial.Path);
			else
				return uri.ToString().Split('?')[0];
		}
    }
}
