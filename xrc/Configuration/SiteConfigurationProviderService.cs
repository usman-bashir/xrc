using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace xrc.Configuration
{
    public class SiteConfigurationProviderService : ISiteConfigurationProviderService
    {
        private Dictionary<string, ISiteConfiguration> _sites = new Dictionary<string,ISiteConfiguration>();

        public SiteConfigurationProviderService(XrcSection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            foreach (SiteElement element in section.Sites)
            {
                _sites.Add(element.Key, GetSiteFromConfig(section, element));
            }
        }

        public ISiteConfiguration GetSiteFromUri(Uri uri)
        {
            foreach (var site in _sites.Values)
            {
                //Note: Uri.IsBaseOf returns true also when called with
                // new Uri("http://contoso.com/path").IsBaseOf(new Uri("http://contoso.com"))
                // Which for me is wrong...so I check also the path
                if (site.Uri.IsBaseOfWithPath(uri))
                    return site;

                if (uri.Scheme == Uri.UriSchemeHttps && site.SecureUri.IsBaseOfWithPath(uri))
                    return site;
            }

            throw new ApplicationException(string.Format("Site configuration for uri '{0}' not found.", uri));
        }

        public ISiteConfiguration GetSiteFromKey(string siteKey)
        {
            var site = _sites[siteKey];
            if (site == null)
                throw new ApplicationException(string.Format("Site '{0}' not found.", siteKey));

            return site;
        }

        private ISiteConfiguration GetSiteFromConfig(XrcSection section, SiteElement element)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            foreach (SiteParameterElement param in section.Parameters)
                parameters[param.Key] = param.Value;
            foreach (SiteParameterElement param in element.Parameters)
                parameters[param.Key] = param.Value;

            var configuration = new SiteConfiguration(element.Key, element.Uri, parameters, element.SecureUri);
            return configuration;
        }
    }
}
