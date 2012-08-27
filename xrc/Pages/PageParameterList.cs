using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages
{
    public class PageParameterList : IEnumerable<PageParameter>
    {
        private Dictionary<string, PageParameter> _parameters = new Dictionary<string, PageParameter>(StringComparer.OrdinalIgnoreCase);

        public PageParameterList()
        {
        }

        public IEnumerator<PageParameter> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        public PageParameter this[string name]
        {
            get
            {
                return _parameters[name];
            }
            set
            {
                if (name != value.Name)
                    throw new ArgumentException("Parameter name doesn't match PageParameter.Name value.");

                _parameters[name] = value;
            }
        }

        public void Add(PageParameter item)
        {
            _parameters.Add(item.Name, item);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }
    }
}
