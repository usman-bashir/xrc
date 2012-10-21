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
		readonly HttpCookieCollection _cookies;
		readonly NameValueCollection _form;
		readonly NameValueCollection _headers;
        readonly NameValueCollection _serverVariables;
		readonly NameValueCollection _queryString;
		readonly XrcUrl _url;
		readonly Encoding _contentEncoding;
		readonly string _httpMethod;

        readonly HttpRequestBase _innerRequest;

		public XrcRequest(HttpRequestBase innerRequest)
		{
			if (innerRequest == null)
				throw new ArgumentNullException("innerRequest");

			_httpMethod = innerRequest.HttpMethod;
			_url = new xrc.XrcUrl(innerRequest.AppRelativeCurrentExecutionFilePath);
			_queryString = innerRequest.QueryString;
			_innerRequest = innerRequest;
			_cookies = _innerRequest.Cookies;
			_form = _innerRequest.Form;
			_headers = _innerRequest.Headers;
			_serverVariables = _innerRequest.ServerVariables;
			_contentEncoding = _innerRequest.ContentEncoding;
		}

		public XrcRequest(XrcUrl requestUrl, string httpMethod = "GET", HttpRequestBase parentRequest = null)
		{
			if (requestUrl == null)
				throw new ArgumentNullException("request");

			_httpMethod = httpMethod;
			_url = requestUrl;
            _innerRequest = parentRequest;

			if (_innerRequest == null)
			{
				_cookies = new HttpCookieCollection();
				_form = new NameValueCollection();
				_headers = new NameValueCollection();
				_serverVariables = new NameValueCollection();
				_contentEncoding = Encoding.UTF8;
			}
			else
			{
				_cookies = _innerRequest.Cookies;
				_form = _innerRequest.Form;
				_headers = _innerRequest.Headers;
				_serverVariables = _innerRequest.ServerVariables;
				_contentEncoding = _innerRequest.ContentEncoding;
			}

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
				return Form.AllKeys
						.Union(QueryString.AllKeys)
						.ToArray();
			}
		}

		public override string this[string key]
		{
			get
			{
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

		public XrcUrl XrcUrl
		{
			get { return _url; }
		}

		public override Uri Url
		{
			get 
			{
				if (_innerRequest != null)
					return _innerRequest.Url;
				else
					return null; 
			}
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

        public override bool IsLocal
        {
            get
            {
                if (_innerRequest == null)
                    return true;
                else
                    return _innerRequest.IsLocal;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                if (_innerRequest == null)
                    return string.Empty;
                else
                    return _innerRequest.ApplicationPath;
            }
        }

        public override NameValueCollection ServerVariables
        {
            get
            {
                return _serverVariables;
            }
        }

		public override string ToString()
		{
			if (Url != null)
				return string.Format("{0} {1}", HttpMethod, Url);
			else
				return base.ToString();
		}
	}
}
