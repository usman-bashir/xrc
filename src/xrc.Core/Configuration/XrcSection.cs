using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using xrc.CustomErrors;

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

        [ConfigurationProperty("customErrors")]
		public CustomErrorCollection CustomErrors
        {
			get { return this["customErrors"] as CustomErrorCollection; }
        }

        [ConfigurationProperty("modules")]
        public ComponentCollection Modules
        {
            get { return this["modules"] as ComponentCollection; }
        }

        [ConfigurationProperty("views")]
        public ComponentCollection Views
        {
            get { return this["views"] as ComponentCollection; }
        }

		[ConfigurationProperty("rootPath")]
		public RootPathElement RootPath
		{
			get { return this["rootPath"] as RootPathElement; }
		}
	}
}
