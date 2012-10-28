using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace xrc.Sites
{
    public class SiteConfigurationProviderService : ISiteConfigurationProviderService
    {
        private Dictionary<string, ISiteConfiguration> _sites = new Dictionary<string,ISiteConfiguration>();

        public SiteConfigurationProviderService(Configuration.ISitesConfig config)
        {
			if (config == null)
				throw new ArgumentNullException("config");

			foreach (var item in config.Sites)
                _sites.Add(item.Key, item);
        }

        public ISiteConfiguration GetSiteFromUri(Uri uri)
        {
            foreach (var site in _sites.Values)
            {
				if (site.MatchUrl(uri))
					return site;
            }

			throw new SiteConfigurationNotFoundException(string.Format("Site configuration for uri '{0}' not found.", uri));
        }

        public ISiteConfiguration GetSiteFromKey(string siteKey)
        {
            var site = _sites[siteKey];
            if (site == null)
				throw new SiteConfigurationNotFoundException(string.Format("Site '{0}' not found.", siteKey));

            return site;
        }
    }
}
