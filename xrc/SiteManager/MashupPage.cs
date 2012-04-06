using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class MashupPage : IEnumerable<MashupAction>
    {
        private Dictionary<string, MashupAction> _list = new Dictionary<string, MashupAction>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _pageParameters = new Dictionary<string, string>();

        public MashupPage()
        {
        }

        public void Add(MashupAction item)
        {
            _list.Add(item.Method, item);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public MashupAction this[string method]
        {
            get
            {
                if (method == null)
                    return null;

                return _list[method];
            }
        }

        public IEnumerator<MashupAction> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item.Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        public Dictionary<string, string> PageParameters
        {
            get { return _pageParameters; }
        }
    }
}
