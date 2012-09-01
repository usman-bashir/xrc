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
                return _views[name];
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
