using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages
{
    public class PageActionList : IEnumerable<PageAction>
    {
        private Dictionary<string, PageAction> _list = new Dictionary<string, PageAction>(StringComparer.OrdinalIgnoreCase);

        public PageActionList()
        {
        }

        public void Add(PageAction item)
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

        public PageAction this[string method]
        {
            get
            {
                if (method == null)
                    return null;

                return _list[method];
            }
        }

        public IEnumerator<PageAction> GetEnumerator()
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
