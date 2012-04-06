using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class RendererDefinition : XClass
    {
        private Type _type;
        private string _slot;

        public RendererDefinition(Type type, string slot)
        {
            if (slot == null)
                slot = string.Empty;

            _type = type;
            _slot = slot.ToLower();
        }

        public Type RendererType
        {
            get { return _type; }
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
