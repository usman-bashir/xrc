using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Sites
{
    public class SiteConfiguration : ISiteConfiguration
    {
		public SiteConfiguration(string key, Uri uri)
			:this(key, uri, null, null)
		{
		}

        public SiteConfiguration(string key, Uri uri,
                                IDictionary<string, string> parameters,
                                Uri secureUri = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
            if (uri == null)
                throw new ArgumentNullException("uri");
			if (parameters == null)
				parameters = new Dictionary<string, string>();
            if (!uri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not absolute.", uri));
            if (secureUri != null && !secureUri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not absolute.", secureUri));

            Key = key;
            Uri = uri.ToLower().AppendSlash();
            Parameters = parameters;

            if (secureUri == null)
                SecureUri = Uri.ToSecure();
            else
                SecureUri = secureUri.ToLower().AppendSlash();
        }

        public string Key
        {
            get; 
            private set;
        }

        public Uri Uri
        {
            get;
            private set;
        }

        public IDictionary<string, string> Parameters
        {
            get; 
            private set;
        }

        public Uri SecureUri
        {
            get;
            private set;
        }

        public Uri GetRelativeUrl(Uri absoluteUrl)
        {
            if (!absoluteUrl.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Url '{0}' is not absolute.", absoluteUrl));

            absoluteUrl = absoluteUrl.ToLower();

            Uri resultUri;
            if (SecureUri.IsBaseOfWithPath(absoluteUrl))
                resultUri = absoluteUrl.MakeRelativeUriEx(SecureUri);
            else if (Uri.IsBaseOfWithPath(absoluteUrl))
                resultUri = absoluteUrl.MakeRelativeUriEx(Uri);
            else
				throw new SiteConfigurationMismatchException(string.Format("Uri '{0}' is not valid for site '{1}'.", absoluteUrl, Key));

			System.Diagnostics.Debug.Assert(!resultUri.IsAbsoluteUri);

            return resultUri;
        }

		public Uri GetAbsoluteUrl(string virtualUrl, Uri contextUrl)
        {
            // Similar code of UrlHelper.Content Method

			if (contextUrl == null || !contextUrl.IsAbsoluteUri)
				throw new UriFormatException(string.Format("Context url '{0}' is not valid. Expected an absolute url.", contextUrl));

			System.Uri resultUri;
			if (virtualUrl.StartsWith("~"))
			{
				virtualUrl = virtualUrl.Substring(1);
				if (SecureUri.IsBaseOfWithPath(contextUrl))
					resultUri = SecureUri.Combine(virtualUrl);
				else if (Uri.IsBaseOfWithPath(contextUrl))
					resultUri = Uri.Combine(virtualUrl);
				else
					throw new SiteConfigurationMismatchException(string.Format("Uri '{0}' is not valid for site '{1}'.", virtualUrl, Key));
			}
			else
				resultUri = new System.Uri(contextUrl, virtualUrl);

			System.Diagnostics.Debug.Assert(resultUri.IsAbsoluteUri);

			return resultUri;
        }
    }
}
