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
        public XProperty(PropertyInfo propertyInfo, object value)
        {
            PropertyInfo = propertyInfo;
            _value = value;
        }

        public XProperty(PropertyInfo propertyInfo, IScriptExpression expression)
        {
            PropertyInfo = propertyInfo;
            _expression = expression;
        }

        public PropertyInfo PropertyInfo
        {
            get;
            private set;
        }

        private object _value;
        public object Value
        {
            get { return _value; }
        }

        private IScriptExpression _expression;
        public IScriptExpression Expression
        {
            get { return _expression; }
        }

    }
}
