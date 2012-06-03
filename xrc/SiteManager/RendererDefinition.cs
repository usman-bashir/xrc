using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class RendererDefinition : XClass
    {
        private ComponentDefinition _component;
        private string _slot;

        public RendererDefinition(ComponentDefinition component, string slot)
        {
            if (slot == null)
                slot = string.Empty;

            _component = component;
            _slot = slot.ToLower();
        }

        public ComponentDefinition Component
        {
            get { return _component; }
        }

        public string Slot
        {
            get
            {
                return _slot;
            }
        }
    }
}
