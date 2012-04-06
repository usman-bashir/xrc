using System;
using System.Xml.Linq;

namespace xrc.Modules
{
    public interface IFileModule
    {
        XDocument Xml(string file);
		XDocument XHtml(string file);
	}
}
