using System;
using System.Web.Routing;
namespace xrc.Modules
{
    public interface IUrlModule
    {
		string Content(string contentPath);
		string Content(string contentPathBase, string contentPath);

		string Current();

		string Initiator();

		bool IsBaseOf(string baseUrl, string url);
    }
}
