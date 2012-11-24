using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages
{
    public class ViewDefinitionList : IEnumerable<ViewDefinition>
    {
        private Dictionary<string, ViewDefinition> _views = new Dictionary<string, ViewDefinition>(StringComparer.OrdinalIgnoreCase);

        public ViewDefinitionList()
        {
        }

        public IEnumerator<ViewDefinition> GetEnumerator()
        {
            return _views.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _views.Values.GetEnumerator();
        }

        public ViewDefinition this[string name]
        {
            get
            {
				ViewDefinition view;
				if (!_views.TryGetValue(name, out view))
					if (string.IsNullOrEmpty(name))
						throw new XrcException("Cannot find a valid default view.");
					else
						throw new XrcException(string.Format("Cannot find a valid view with name '{0}'.", name));

				return view;
            }
        }

        public void Add(ViewDefinition item)
        {
            _views.Add(item.Slot, item);
        }

        public int Count
        {
            get { return _views.Count; }
        }
    }
}
