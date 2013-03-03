using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
    public class PageDirectoryList : IEnumerable<PageDirectory>
	{
        readonly Dictionary<string, PageDirectory> _list = new Dictionary<string, PageDirectory>(StringComparer.OrdinalIgnoreCase);
        readonly PageDirectory _parent;

        public PageDirectoryList(PageDirectory parent)
		{
			_parent = parent;
		}

        public void Add(PageDirectory item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (_list.ContainsKey(item.ResourceName))
				throw new DuplicateItemException(item.ResourceName);

			_list.Add(item.ResourceName, item);
			item.Parent = _parent;
		}

        public void AddRange(IEnumerable<PageDirectory> items)
		{
			foreach (var i in items)
				Add(i);
		}

        public IEnumerator<PageDirectory> GetEnumerator()
		{
			return _list.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

        public PageDirectory this[string resourceName]
		{
			get
			{
                PageDirectory item;
				if (_list.TryGetValue(resourceName, out item))
					return item;

				return null;
			}
		}
	}

}
