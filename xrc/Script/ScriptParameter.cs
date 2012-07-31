using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.Script
{
    public class ScriptParameter
    {
        public ScriptParameter(string name, Type type, object value = null)
        {
            _name = name;
            _type = type;
            _value = value;
        }

        private Type _type;
        public Type Type
        {
            get { return _type; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
