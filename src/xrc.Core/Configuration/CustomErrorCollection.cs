using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
	public class CustomErrorCollection : ConfigurationElementCollection
    {
		[ConfigurationProperty("defaultUrl", IsRequired = true)]
		public string DefaultUrl
		{
			get { return (string)this["defaultUrl"]; }
			set { this["defaultUrl"] = value; }
		}

		public CustomErrorElement this[int statusCode]
        {
            get
            {
				return base.BaseGet(statusCode) as CustomErrorElement;
            }
            set
            {
				if (base.BaseGet(statusCode) != null)
                {
					base.BaseRemoveAt(statusCode);
                }
				this.BaseAdd(statusCode, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
			return new CustomErrorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
			return ((CustomErrorElement)element).StatusCode;
        }

		public CustomErrorElement GetElementByKey(string key)
        {
			foreach (CustomErrorElement item in this)
            {
                if (key == item.StatusCode.ToString())
                    return item;
            }

            return null;
        }

        public void Add(CustomErrorElement element)
        {
            base.BaseAdd(element);
        }
    }
}
