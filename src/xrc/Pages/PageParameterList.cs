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
				if (!string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
					throw new ArgumentException("Name doesn't match PageParameter.Name value.");

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

		public bool TryGetValue(string parameter, out PageParameter pageParam)
		{
			return _parameters.TryGetValue(parameter, out pageParam);
		}
	}
}
