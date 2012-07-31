using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class MashupParameterList : IEnumerable<MashupParameter>
    {
        private Dictionary<string, MashupParameter> _parameters = new Dictionary<string, MashupParameter>(StringComparer.OrdinalIgnoreCase);

        public MashupParameterList()
        {
        }

        public IEnumerator<MashupParameter> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        public MashupParameter this[string name]
        {
            get
            {
                return _parameters[name];
            }
            set
            {
                if (name != value.Name)
                    throw new ArgumentException("Parameter name doesn't match MashupParameter.Name value.");

                _parameters[name] = value;
            }
        }

        public void Add(MashupParameter item)
        {
            _parameters.Add(item.Name, item);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }
    }
}
