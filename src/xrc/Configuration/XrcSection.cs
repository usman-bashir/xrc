using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
	public class XrcSection : ConfigurationSection, IModuleConfig, IViewConfig, ISitesConfig
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

		IEnumerable<Sites.ISiteConfiguration> ISitesConfig.Sites
		{
			get 
			{
				foreach (SiteElement siteElement in Sites)
				{
					yield return GetSiteFromConfig(siteElement);
				}
			}
		}

		private Sites.ISiteConfiguration GetSiteFromConfig(SiteElement element)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();

			foreach (SiteParameterElement param in Parameters)
				parameters[param.Key] = param.Value;
			foreach (SiteParameterElement param in element.Parameters)
				parameters[param.Key] = param.Value;

			var configuration = new Sites.SiteConfiguration(element.Key, element.Uri, parameters, element.SecureUri);
			return configuration;
		}
	}
}
