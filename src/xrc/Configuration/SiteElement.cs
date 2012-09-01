using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class SiteElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public Uri Uri
        {
            get { return (Uri)this["uri"]; }
            set { this["uri"] = value; }
        }

        [ConfigurationProperty("secureUri")]
        public Uri SecureUri
        {
            get { return (Uri)this["secureUri"]; }
            set { this["secureUri"] = value; }
        }

        [ConfigurationProperty("parameters")]
        public SiteParameterCollection Parameters
        {
            get { return this["parameters"] as SiteParameterCollection; }
        }
    }
}
