using System;

namespace xrc.Modules
{
    public interface IUrlModule
    {
        string Content(string contentPath);
        string Content(string baseUri, string uri);
        string Content(string baseUri, string uri1, string uri2);
    }
}
