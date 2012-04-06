using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
    public class SiteConfiguration : ISiteConfiguration
    {
        public SiteConfiguration(string key, Uri uri,
                                IDictionary<string, string> parameters,
                                Uri secureUri = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
            if (uri == null)
                throw new ArgumentNullException("uri");
            if (parameters == null)
                throw new ArgumentNullException("parameters");
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

        public Uri GetRelativeUri(Uri absoluteUri)
        {
            if (!absoluteUri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not absolute.", absoluteUri));

            absoluteUri = absoluteUri.ToLower();

            Uri resultUri;
            if (SecureUri.IsBaseOfWithPath(absoluteUri))
                resultUri = absoluteUri.MakeRelativeUriEx(SecureUri);
            else if (Uri.IsBaseOfWithPath(absoluteUri))
                resultUri = absoluteUri.MakeRelativeUriEx(Uri);
            else
                throw new ApplicationException(string.Format("Uri '{0}' is not valid for the site '{1}'.", absoluteUri, Key));

            if (resultUri.IsAbsoluteUri)
                throw new ApplicationException(string.Format("Uri '{0}' is not valid for the site '{1}'.", absoluteUri, Key));

            return resultUri;
        }

		public string UrlContent(string contentUri, Uri contextUri)
        {
            // Similar code of UrlHelper.Content Method

			if (contextUri == null || !contextUri.IsAbsoluteUri)
				throw new ApplicationException(string.Format("Context uri '{0}' is not valid. Expected an absolute url.", contextUri));

			System.Uri resultUri;
			if (contentUri.StartsWith("~"))
			{
				contentUri = contentUri.Substring(1);
				if (SecureUri.IsBaseOfWithPath(contextUri))
					resultUri = SecureUri.Combine(contentUri);
				else if (Uri.IsBaseOfWithPath(contextUri))
					resultUri = Uri.Combine(contentUri);
				else
					throw new ApplicationException(string.Format("Uri '{0}' is not valid for the site '{1}'.", contentUri, Key));
			}
			else
				resultUri = new System.Uri(contextUri, contentUri);

			return resultUri.ToString();
        }
    }
}
