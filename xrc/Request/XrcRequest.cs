using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.IO;

namespace xrc
{
	// TODO Verificare meglio implementazione di queste classi e codice classi HttpResponseBase, HttpRequestBase

	public class XrcRequest : HttpRequestBase
	{
		private HttpCookieCollection _cookies = new HttpCookieCollection();
		private NameValueCollection _form = new NameValueCollection();
		private NameValueCollection _headers = new NameValueCollection();
		private NameValueCollection _queryString;
		private Uri _url;
		private Encoding _contentEncoding;
		private string _httpMethod;

		public XrcRequest(Uri request, Encoding encoding = null, string httpMethod = "GET")
		{
			if (request == null)
				throw new ArgumentNullException("request");

			if (encoding == null)
				encoding = Encoding.UTF8;

			_httpMethod = httpMethod;
			_contentEncoding = encoding;
			_url = request;
			_queryString = HttpUtility.ParseQueryString(_url.Query, _contentEncoding);
		}

		//public override string[] AcceptTypes
		//{
		//    get;
		//    set;
		//}

		//public override int ContentLength
		//{
		//    get;
		//    set;
		//}

		public override Encoding ContentEncoding
		{
			get { return _contentEncoding; }
		}

		//public override string ContentType
		//{
		//    get;
		//    set;
		//}

		public override HttpCookieCollection Cookies
		{
			get { return _cookies; }
		}

		public override NameValueCollection Form
		{
			get { return _form; }
		}

		public override NameValueCollection QueryString
		{
			get { return _queryString; }
		}

		public override NameValueCollection Headers
		{
			get { return _headers; }
		}

		public string[] AllKeys
		{
			get
			{
				// Parameters precedence
				// TODO Add Custom parameters
				// QueryString
				// Form
				// TODO Add Segments parameters

				return Form.AllKeys
						.Union(QueryString.AllKeys)
						.ToArray();
			}
		}

		public override string this[string key]
		{
			get
			{
				// Parameters precedence
				// TODO Add Custom parameters
				// QueryString
				// Form
				// TODO Add Segments parameters

				string val = _form[key];
				if (val == null)
					val = _queryString[key];

				return val;
			}
		}

		public override string HttpMethod
		{
			get { return _httpMethod; }
		}

		//public override Stream InputStream
		//{
		//    get;
		//    set;
		//}

		//public override bool IsLocal
		//{
		//    get;
		//    set;
		//}

		//public override bool IsSecureConnection
		//{
		//    get;
		//    set;
		//}

		public override Uri Url
		{
			get { return _url; }
		}

		//public override Uri UrlReferrer
		//{
		//    get;
		//    set;
		//}

		//public override string UserAgent
		//{
		//    get;
		//    set;
		//}

		//public override string UserHostAddress
		//{
		//    get;
		//    set;
		//}

		//public override string[] UserLanguages
		//{
		//    get;
		//    set;
		//}

		public override string ToString()
		{
			if (Url != null)
				return string.Format("{0} {1}", HttpMethod, Url);
			else
				return base.ToString();
		}
	}
}
