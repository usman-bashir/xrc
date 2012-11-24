using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace xrc.Pages
{
    public class UriSegmentParameterList : IDictionary<string, string>
    {
        private Dictionary<string, string> _list = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public UriSegmentParameterList()
        {
        }

		public UriSegmentParameterList(IDictionary<string, string> list)
		{
			foreach (var i in list)
				Add(i.Key, i.Value);
		}

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        public string this[string name]
        {
            get
            {
                return _list[name];
            }
            set
            {
                _list[name] = value;
            }
        }

        public void Add(string name, string value)
        {
			if (name == null)
				throw new ArgumentNullException("name");

			if (_list.ContainsKey(name))
				throw new DuplicateItemException(name);

            _list.Add(name, value);
        }

		public void AddRange(IDictionary<string, string> range)
		{
			foreach (var i in range)
				Add(i.Key, i.Value);
		}

        public int Count
        {
            get { return _list.Count; }
        }

		public bool TryGetValue(string name, out string value)
		{
			return _list.TryGetValue(name, out value);
		}

		public bool ContainsKey(string key)
		{
			return _list.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return _list.Keys; }
		}

		public bool Remove(string key)
		{
			return _list.Remove(key);
		}

		public ICollection<string> Values
		{
			get { return _list.Values; }
		}

		public void Add(KeyValuePair<string, string> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return _list.ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, string>>)_list).CopyTo(array, arrayIndex);
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<string, string>>)_list).IsReadOnly; }
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			return _list.Remove(item.Key);
		}
	}
}
