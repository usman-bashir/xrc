﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public class ModuleDefinitionList : IEnumerable<ModuleDefinition>
    {
        private Dictionary<string, ModuleDefinition> _list = new Dictionary<string, ModuleDefinition>(StringComparer.OrdinalIgnoreCase);

        public ModuleDefinitionList()
        {
        }

        public void Add(ModuleDefinition item)
        {
            _list.Add(item.Name, item);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public ModuleDefinition this[string name]
        {
            get
            {
                if (name == null)
                    return null;

                return _list[name];
            }
			set
			{
				if (!string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
					throw new ArgumentException("Name doesn't match ModuleDefinition.Name value.");

				_list[name] = value;
			}
        }

        public IEnumerator<ModuleDefinition> GetEnumerator()
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

		public bool ContainsKey(string name)
		{
			return _list.ContainsKey(name);
		}
	}
}
