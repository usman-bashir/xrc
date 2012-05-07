using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class XrcSection : ConfigurationSection
    {
        public static XrcSection GetSection()
        {
            return GetSection(true);
        }

        public static XrcSection GetSection(bool throwIfNotFound)
        {
            XrcSection section =
                (XrcSection)ConfigurationManager.GetSection("xrc");

            if (section == null && throwIfNotFound)
				throw new ApplicationException("xrc configuration section not found");

            return section;
        }

        [ConfigurationProperty("sites")]
        public SiteCollection Sites
        {
            get { return this["sites"] as SiteCollection; }
        }

        [ConfigurationProperty("parameters")]
        public SiteParameterCollection Parameters
        {
            get { return this["parameters"] as SiteParameterCollection; }
        }

        [ConfigurationProperty("modules")]
        public ComponentCollection Modules
        {
            get { return this["modules"] as ComponentCollection; }
        }

        [ConfigurationProperty("renderers")]
        public ComponentCollection Renderers
        {
            get { return this["renderers"] as ComponentCollection; }
        }
    }
}
