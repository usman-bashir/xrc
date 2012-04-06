using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace d3cms
{
	public class CmsHttpResponse : IResponse
	{
		private HttpResponse _httpResponse;

		public CmsHttpResponse(HttpResponse httpResponse)
		{
			if (httpResponse == null)
				throw new ArgumentNullException("httpResponse");

			_httpResponse = httpResponse;
		}

		public Encoding ContentEncoding
		{
			get { return _httpResponse.ContentEncoding; }
		}

		public string ContentType
		{
			get { return _httpResponse.ContentType; }
			set { _httpResponse.ContentType = value; }
		}

		public HttpCookieCollection Cookies
		{
			get { return _httpResponse.Cookies; }
		}

		public System.Collections.Specialized.NameValueCollection Headers
		{
			get { return _httpResponse.Headers; }
		}

		public System.IO.TextWriter Output
		{
			get { return _httpResponse.Output; }
		}

		public System.IO.Stream OutputStream
		{
			get { return _httpResponse.OutputStream; }
		}

		public string RedirectLocation
		{
			get { return _httpResponse.RedirectLocation; }
			set { _httpResponse.RedirectLocation = value; }
		}

		public HttpStatusCode StatusCode
		{
			get { return (HttpStatusCode)_httpResponse.StatusCode; }
			set { _httpResponse.StatusCode = (int)value; }
		}

		public string StatusDescription
		{
			get { return _httpResponse.StatusDescription; }
			set { _httpResponse.StatusDescription = value; }
		}
	}
}
