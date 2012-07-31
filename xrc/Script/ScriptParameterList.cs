using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Script
{
    public class ScriptParameterList : IEnumerable<ScriptParameter>
    {
        private Dictionary<string, ScriptParameter> _parameters = new Dictionary<string, ScriptParameter>(StringComparer.Ordinal);

        public ScriptParameterList()
        {
        }

        public IEnumerator<ScriptParameter> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        public ScriptParameter this[string name]
        {
            get
            {
                return _parameters[name];
            }
        }

        public void Add(ScriptParameter item)
        {
            _parameters.Add(item.Name, item);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }
    }
}
