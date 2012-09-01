using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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

        public ContentResult Result
        {
            get;
            set;
        }
    }

    public delegate void RenderSlotEventHandler(object sender, RenderSlotEventArgs e);
}
