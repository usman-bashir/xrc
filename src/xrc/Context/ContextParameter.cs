using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc
{
    public class ContextParameter
    {
        public ContextParameter(string name, Type type, object value)
        {
            _name = name;
            _value = value;
            _type = type;
        }

        private Type _type;
        public Type Type
        {
            get { return _type; }
        }

        private object _value;
        public object Value
        {
            get { return _value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }
    }
}
