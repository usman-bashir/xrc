using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.Providers.Common
{
	public class XrcItemList : IEnumerable<XrcItem>
	{
		readonly Dictionary<string, XrcItem> _list = new Dictionary<string, XrcItem>(StringComparer.OrdinalIgnoreCase);
		readonly XrcItem _parent;

		public XrcItemList(XrcItem parent)
		{
			_parent = parent;
		}

		public void Add(XrcItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (_list.ContainsKey(item.ResourceName))
				throw new DuplicateItemException(item.ResourceName);

			_list.Add(item.ResourceName, item);
			item.Parent = _parent;
		}

		public void AddRange(IEnumerable<XrcItem> items)
		{
			foreach (var i in items)
				Add(i);
		}

		public IEnumerator<XrcItem> GetEnumerator()
		{
			return _list.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public XrcItem this[string resourceName]
		{
			get
			{
				XrcItem item;
				if (_list.TryGetValue(resourceName, out item))
					return item;

				return null;
			}
		}
	}

}
