using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace d3cms
{
    public class CmsHttpRequest : IRequest
    {
        private HttpRequest _httpRequest;

        public CmsHttpRequest(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public string[] AcceptTypes
        {
            get { return _httpRequest.AcceptTypes; }
        }

        public int ContentLength
        {
            get { return _httpRequest.ContentLength; }
        }

        public string ContentType
        {
            get { return _httpRequest.ContentType; }
        }

        public System.Web.HttpCookieCollection Cookies
        {
            get { return _httpRequest.Cookies; }
        }

        public System.Collections.Specialized.NameValueCollection Form
        {
            get { return _httpRequest.Form; }
        }

        public System.Collections.Specialized.NameValueCollection Headers
        {
            get { return _httpRequest.Headers; }
        }

        public System.Collections.Specialized.NameValueCollection QueryString
        {
            get { return _httpRequest.QueryString; }
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

        public string this[string key]
        {
            get 
            {
                // Parameters precedence
                // TODO Add Custom parameters
                // QueryString
                // Form
                // TODO Add Segments parameters

                string val = Form[key];
                if (val == null)
                    val = QueryString[key];

                return val;
            }
        }

        public string HttpMethod
        {
            get { return _httpRequest.HttpMethod; }
        }

        public System.IO.Stream InputStream
        {
            get { return _httpRequest.InputStream; }
        }

        public bool IsLocal
        {
            get { return _httpRequest.IsLocal; }
        }

        public bool IsSecureConnection
        {
            get { return _httpRequest.IsSecureConnection; }
        }

        public Uri Url
        {
            get { return _httpRequest.Url; }
        }

        public Uri UrlReferrer
        {
            get { return _httpRequest.UrlReferrer; }
        }

        public string UserAgent
        {
            get { return _httpRequest.UserAgent; }
        }

        public string UserHostAddress
        {
            get { return _httpRequest.UserHostAddress; }
        }

        public string[] UserLanguages
        {
            get { return _httpRequest.UserLanguages; }
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
