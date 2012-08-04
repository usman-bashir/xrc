using System;
namespace xrc.Modules
{
    public interface IHtmlModule : IModule
    {
        string Page(string url);
        void RenderPage(string url, System.IO.Stream output);
    }
}
