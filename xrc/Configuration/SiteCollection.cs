using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class SiteCollection : ConfigurationElementCollection
    {
        public SiteElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as SiteElement;
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
            return new SiteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteElement)element).Key;
        }

        public SiteElement GetElementByKey(string key)
        {
            foreach (SiteElement item in this)
            {
                if (string.Equals(key, item.Key, StringComparison.InvariantCultureIgnoreCase))
                    return item;
            }

            return null;
        }

        public void Add(SiteElement element)
        {
            base.BaseAdd(element);
        }
    }
}
