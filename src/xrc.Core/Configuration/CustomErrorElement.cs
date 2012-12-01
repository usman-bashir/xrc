using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class CustomErrorElement : ConfigurationElement
    {
		[ConfigurationProperty("statusCode", IsRequired = true)]
        public int StatusCode
        {
			get { return (int)this["statusCode"]; }
			set { this["statusCode"] = value; }
        }
		[ConfigurationProperty("url", IsRequired = true)]
		public string Url
		{
			get { return (string)this["url"]; }
			set { this["url"] = value; }
		}
    }
}
