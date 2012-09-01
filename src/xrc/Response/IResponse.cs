using System;
namespace d3cms
{
	public interface IResponse
	{
		System.Text.Encoding ContentEncoding { get; }
		string ContentType { get; set; }
		System.Web.HttpCookieCollection Cookies { get; }
		System.Collections.Specialized.NameValueCollection Headers { get; }
		System.IO.TextWriter Output { get; }
		System.IO.Stream OutputStream { get; }
		string RedirectLocation { get; set; }
		System.Net.HttpStatusCode StatusCode { get; set; }
		string StatusDescription { get; set; }
	}
}
