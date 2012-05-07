using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
    public class ComponentElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [System.ComponentModel.TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty("type", IsRequired = false)]
        public Type @Type
        {
            get { return (Type)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("assembly", IsRequired = false)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
    }
}
