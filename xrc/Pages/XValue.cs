using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using xrc.Script;

namespace xrc.Pages
{
    public class XValue
    {
        public XValue(Type valueType, object value)
        {
            if (valueType == null)
                throw new ArgumentNullException("valueType");

            if (value != null && !valueType.IsAssignableFrom(value.GetType()))
                throw new ApplicationException(string.Format("Cannot convert value '{1}' to type '{0}'.", valueType, value));

            ValueType = valueType;
            _value = value;
        }

        public XValue(Type valueType, IScriptExpression expression)
        {
            if (valueType == null)
                throw new ArgumentNullException("valueType");
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!valueType.IsAssignableFrom(expression.ReturnType))
                throw new ApplicationException(string.Format("Cannot convert type '{1}' to type '{0}'.", valueType, expression.ReturnType));

            ValueType = valueType;
            _expression = expression;
        }

        public Type ValueType
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

        public bool IsEmpty()
        {
            return _expression == null && _value == null;
        }

        public override string ToString()
        {
            if (_expression != null)
                return _expression.ToString();
            else if (_value != null)
                return _value.ToString();

            return base.ToString();
        }
    }
}
