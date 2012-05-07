using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class ComponentCollection : ConfigurationElementCollection
    {
        public ComponentElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ComponentElement;
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
            return new ComponentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ComponentElement)element).Name;
        }

        public ComponentElement GetElementByKey(string key)
        {
            foreach (ComponentElement item in this)
            {
                if (string.Equals(key, item.Name, StringComparison.InvariantCultureIgnoreCase))
                    return item;
            }

            return null;
        }

        public void Add(ComponentElement element)
        {
            base.BaseAdd(element);
        }
    }
}
