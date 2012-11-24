using System;
using xrc.Pages;
namespace xrc.Modules
{
    public interface IPageModule
    {
		IPage GetCurrent();
		IPage Get(string url);
		IPage GetInitiator();
		object GetPageParameter(string url, string parameter);
		object GetPageParameterXslt(string url, string parameter);
	}
}
