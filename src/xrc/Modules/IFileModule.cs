using System;
namespace xrc.Modules
{
    public interface IFileModule
    {
        System.Xml.Linq.XDocument XHtml(string file);
        System.Xml.Linq.XDocument Xml(string file);
		string Html(string file);
		string Text(string file);
	}
}
