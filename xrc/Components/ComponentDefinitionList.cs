using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    public class ComponentDefinitionList<T> : IEnumerable<ComponentDefinition>
    {
        private Dictionary<string, ComponentDefinition> _list = new Dictionary<string, ComponentDefinition>(StringComparer.OrdinalIgnoreCase);

        public ComponentDefinitionList()
        {
        }

        public void Add(ComponentDefinition item)
        {
            if (!typeof(T).IsAssignableFrom(item.Type))
                throw new ApplicationException(string.Format("Component '{0}' is not of type '{1}'.", item.Type, typeof(T)));

            _list.Add(item.Name, item);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public ComponentDefinition this[string name]
        {
            get
            {
                try
                {
                    if (name == null)
                        return null;

                    return _list[name];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException(string.Format("Component for key '{0}' not found.", name));
                }
            }
        }

        public IEnumerator<ComponentDefinition> GetEnumerator()
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
