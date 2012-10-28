using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.Providers.Common
{
	public class XrcItemList : IEnumerable<XrcItem>
	{
		readonly List<XrcItem> _list = new List<XrcItem>();
		readonly XrcItem _parent;

		public XrcItemList(XrcItem parent)
		{
			_parent = parent;
		}

		public void Add(XrcItem item)
		{
			item.Parent = _parent;
			_list.Add(item);
		}

		public void AddRange(IEnumerable<XrcItem> items)
		{
			foreach (var i in items)
				Add(i);
		}

		public IEnumerator<XrcItem> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}

}
