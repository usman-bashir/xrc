using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using xrc.Script;

namespace xrc.SiteManager
{
    public class XProperty
    {
        public XProperty(PropertyInfo propertyInfo, XValue xvalue)
        {
            _propertyInfo = propertyInfo;
            _xValue = xvalue;
        }

        private PropertyInfo _propertyInfo;
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        private XValue _xValue;
        public XValue Value
        {
            get { return _xValue; }
        }
    }
}
