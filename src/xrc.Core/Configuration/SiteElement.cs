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

        [ConfigurationProperty("uriPattern", IsRequired = true)]
        public string UriPattern
        {
			get { return (string)this["uriPattern"]; }
			set { this["uriPattern"] = value; }
        }

        [ConfigurationProperty("parameters")]
        public SiteParameterCollection Parameters
        {
            get { return this["parameters"] as SiteParameterCollection; }
        }
    }
}
