using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class MashupActionList : IEnumerable<MashupAction>
    {
        private Dictionary<string, MashupAction> _list = new Dictionary<string, MashupAction>(StringComparer.OrdinalIgnoreCase);

        public MashupActionList()
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
    }
}
