using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class MashupAction : IEnumerable<RendererDefinition>
    {
        private Dictionary<string, RendererDefinition> _renderers = new Dictionary<string, RendererDefinition>(StringComparer.OrdinalIgnoreCase);

        public MashupAction(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("method");
            Method = method.ToLower();
        }

        public string Method
        {
            get;
            private set;
        }

        /// <summary>
        /// Define the parent layout file.
        /// </summary>
        public string Parent
        {
            get;
            set;
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
