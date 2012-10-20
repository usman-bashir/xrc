using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

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
            Uri = uri.ToLower().AppendTrailingSlash();
            Parameters = parameters;

            if (secureUri == null)
                SecureUri = Uri.ToSecure();
            else
				SecureUri = secureUri.ToLower().AppendTrailingSlash();
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
    }
}
