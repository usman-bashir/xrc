using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Collections.Specialized;

namespace d3cms
{
    public interface IRequest
    {
        string[] AcceptTypes { get; }
        int ContentLength { get; }
        string ContentType { get; }

        HttpCookieCollection Cookies { get; }
        NameValueCollection Form { get; }
        NameValueCollection Headers { get; }
        NameValueCollection QueryString { get; }

        string[] AllKeys { get; }
        string this[string key] { get; }

        string HttpMethod { get; }
        Stream InputStream { get; }
        bool IsLocal { get; }
        bool IsSecureConnection { get; }
        Uri Url { get; }
        Uri UrlReferrer { get; }
        string UserAgent { get; }
        string UserHostAddress { get; }
        string[] UserLanguages { get; }
    }
}
