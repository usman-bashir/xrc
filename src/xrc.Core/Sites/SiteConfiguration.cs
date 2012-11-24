using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace xrc.Sites
{
    public class SiteConfiguration : ISiteConfiguration
    {
		readonly Regex _patternRegEx;

		public SiteConfiguration(string key, string uriPattern)
			: this(key, uriPattern, null)
		{
		}

        public SiteConfiguration(string key, string uriPattern,
                                IDictionary<string, string> parameters)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
			if (uriPattern == null)
				throw new ArgumentNullException("uriPattern");
			if (parameters == null)
				parameters = new Dictionary<string, string>();

            Key = key;
			UriPattern = uriPattern;
            Parameters = parameters;
			_patternRegEx = new Regex(uriPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public string Key
        {
            get; 
            private set;
        }

        public string UriPattern
        {
            get;
            private set;
        }

        public IDictionary<string, string> Parameters
        {
            get; 
            private set;
        }

		public bool MatchUrl(Uri url)
		{
			return _patternRegEx.Match(url.ToString()).Success;
		}
	}
}
