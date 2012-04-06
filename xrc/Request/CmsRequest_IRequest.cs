using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.IO;

namespace d3cms
{
    public class CmsRequest : IRequest
    {
        private HttpCookieCollection _cookies = new HttpCookieCollection();
        private NameValueCollection _form = new NameValueCollection();
        private NameValueCollection _headers = new NameValueCollection();
        private NameValueCollection _queryString;
        private Uri _url;
        private Encoding _contentEncoding;

        public CmsRequest(Uri request, Encoding encoding = null)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (encoding == null)
                encoding = Encoding.UTF8;

            _contentEncoding = encoding;
            _url = request;
            _queryString = HttpUtility.ParseQueryString(_url.Query, _contentEncoding);
        }

        public static CmsRequest GET(Uri request, Encoding encoding = null)
        {
            CmsRequest cmsRequest = new CmsRequest(request, encoding);

            cmsRequest.ContentLength = 0;
            cmsRequest.HttpMethod = "GET";

            return cmsRequest;
        }

        public string[] AcceptTypes
        {
            get;
            set;
        }

        public int ContentLength
        {
            get;
            set;
        }

        public Encoding ContentEncoding 
        {
            get { return _contentEncoding; }
        }

        public string ContentType
        {
            get;
            set;
        }

        public HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

        public NameValueCollection Form
        {
            get { return _form; }
        }

        public NameValueCollection QueryString 
        {
            get { return _queryString; }
        }

        public NameValueCollection Headers
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

        public string this[string key]
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

        public string HttpMethod
        {
            get;
            set;
        }

        public Stream InputStream
        {
            get;
            set;
        }

        public bool IsLocal
        {
            get;
            set;
        }

        public bool IsSecureConnection
        {
            get;
            set;
        }

        public Uri Url
        {
            get { return _url; }
        }

        public Uri UrlReferrer
        {
            get;
            set;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public string UserHostAddress
        {
            get;
            set;
        }

        public string[] UserLanguages
        {
            get;
            set;
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
