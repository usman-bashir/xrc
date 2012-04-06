using System;

namespace xrc.Modules
{
    public interface IHtmlModule
    {
        void RenderAction(string url, System.IO.Stream output);
        string Action(string url);
    }
}
