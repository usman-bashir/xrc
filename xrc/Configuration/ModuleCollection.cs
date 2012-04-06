//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Configuration;

//namespace xrc.Configuration
//{
//    public class ModuleCollection : ConfigurationElementCollection
//    {
//        public ModuleElement this[int index]
//        {
//            get
//            {
//                return base.BaseGet(index) as ModuleElement;
//            }
//            set
//            {
//                if (base.BaseGet(index) != null)
//                {
//                    base.BaseRemoveAt(index);
//                }
//                this.BaseAdd(index, value);
//            }
//        }

//        protected override ConfigurationElement CreateNewElement()
//        {
//            return new ModuleElement();
//        }

//        protected override object GetElementKey(ConfigurationElement element)
//        {
//            return ((ModuleElement)element).Name;
//        }

//        public ModuleElement GetElementByKey(string key)
//        {
//            foreach (ModuleElement item in this)
//            {
//                if (string.Equals(key, item.Name, StringComparison.InvariantCultureIgnoreCase))
//                    return item;
//            }

//            return null;
//        }

//        public void Add(ModuleElement element)
//        {
//            base.BaseAdd(element);
//        }
//    }
//}
