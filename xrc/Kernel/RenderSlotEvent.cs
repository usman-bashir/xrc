using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    public class RenderSlotEventArgs : EventArgs
    {
        public RenderSlotEventArgs(string name)
        {
            if (name == null)
                name = string.Empty;

            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            set;
        }
    }

    public delegate void RenderSlotEventHandler(object sender, RenderSlotEventArgs e);
}
