using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    public class ContextParameterList : IEnumerable<ContextParameter>
    {
        private Dictionary<string, ContextParameter> _parameters = new Dictionary<string, ContextParameter>(StringComparer.OrdinalIgnoreCase);

        public ContextParameterList()
        {
        }

        public IEnumerator<ContextParameter> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        public ContextParameter this[string name]
        {
            get
            {
                return _parameters[name];
            }
            set
            {
                if (name != value.Name)
                    throw new ArgumentException("Parameter name doesn't match ContextParameter.Name value.");

                _parameters[name] = value;
            }
        }

        public void Add(ContextParameter item)
        {
            _parameters.Add(item.Name, item);
        }

        public bool TryGetValue(string name, out ContextParameter value)
        {
            return _parameters.TryGetValue(name, out value);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }
    }
}
