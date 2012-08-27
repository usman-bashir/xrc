using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages
{
    public abstract class XClass
    {
        private XPropertyList _properties = new XPropertyList();

        public XClass()
        {
        }

        public XPropertyList Properties
        {
            get { return _properties; }
        }
    }
}
