using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class SiteParameterCollection : ConfigurationElementCollection
    {
        public SiteParameterElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as SiteParameterElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteParameterElement)element).Key;
        }

        public SiteParameterElement GetElementByKey(string key)
        {
            foreach (SiteParameterElement item in this)
            {
                if (string.Equals(key, item.Key, StringComparison.InvariantCultureIgnoreCase))
                    return item;
            }

            return null;
        }

        public void Add(SiteParameterElement element)
        {
            base.BaseAdd(element);
        }
    }
}
