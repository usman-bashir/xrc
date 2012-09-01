using System;
namespace xrc.Modules
{
    public interface IFileModule : IModule
    {
        System.Xml.Linq.XDocument XHtml(string file);
        System.Xml.Linq.XDocument Xml(string file);
    }
}
