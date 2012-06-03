using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class RendererDefinitionList : IEnumerable<RendererDefinition>
    {
        private Dictionary<string, RendererDefinition> _renderers = new Dictionary<string, RendererDefinition>(StringComparer.OrdinalIgnoreCase);

        public RendererDefinitionList()
        {
        }

        public IEnumerator<RendererDefinition> GetEnumerator()
        {
            return _renderers.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _renderers.Values.GetEnumerator();
        }

        public RendererDefinition this[string name]
        {
            get
            {
                return _renderers[name];
            }
        }

        public void Add(RendererDefinition item)
        {
            _renderers.Add(item.Slot, item);
        }

        public int Count
        {
            get { return _renderers.Count; }
        }
    }
}
