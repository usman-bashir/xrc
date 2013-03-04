using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
	public class PageFileList : IEnumerable<PageFile>
	{
		readonly Dictionary<string, PageFile> _list = new Dictionary<string, PageFile>(StringComparer.OrdinalIgnoreCase);
        readonly PageDirectory _parent;

        public PageFileList(PageDirectory parent)
		{
			_parent = parent;
		}

        public void Add(PageFile item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (_list.ContainsKey(item.ResourceName))
				throw new DuplicateItemException(item.ResourceName);

			_list.Add(item.ResourceName, item);
			item.Parent = _parent;
		}

        public void AddRange(IEnumerable<PageFile> items)
		{
			foreach (var i in items)
				Add(i);
		}

		public IEnumerator<PageFile> GetEnumerator()
		{
			return _list.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

        public PageFile this[string resourceName]
		{
			get
			{
                PageFile item;
				if (_list.TryGetValue(resourceName, out item))
					return item;

				return null;
			}
		}
	}

}
