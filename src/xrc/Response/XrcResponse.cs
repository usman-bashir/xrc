using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace xrc
{
	// TODO Verificare meglio implementazione di queste classi e codice classi HttpResponseBase, HttpRequestBase
	// verificare anche metodi End, Flush e Close se rilasciano correttamente tutte le risorse.

	public class XrcResponse : HttpResponseBase
	{
		readonly HttpCookieCollection _cookies;
		readonly NameValueCollection _headers;
		readonly TextWriter _output;
		readonly Stream _outputStream;
		readonly bool _isStreamOwner;

		readonly HttpResponseBase _innerResponse;

		int _statusCode;
		Encoding _contentEncoding;
		string _contentType;
		string _redirectLocation;
		string _statusDescription;

		public XrcResponse(HttpResponseBase innerResponse)
		{
			if (innerResponse == null)
				throw new ArgumentNullException("innerResponse");

			_innerResponse = innerResponse;

			_isStreamOwner = false;
			_cookies = _innerResponse.Cookies;
			_headers = _innerResponse.Headers;
			_statusCode = _innerResponse.StatusCode;
			_contentEncoding = _innerResponse.ContentEncoding;
			_contentType = _innerResponse.ContentType;
			_redirectLocation = _innerResponse.RedirectLocation;
			_statusDescription = _innerResponse.StatusDescription;
			_outputStream = _innerResponse.OutputStream;
			_output = _innerResponse.Output;
		}

		public XrcResponse(Stream stream, HttpResponseBase parentResponse = null)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			_innerResponse = parentResponse;

			_isStreamOwner = true;
			if (_innerResponse == null)
			{
				_cookies = new HttpCookieCollection();
				_headers = new NameValueCollection();
				_statusCode = (int)HttpStatusCode.OK;
				_contentEncoding = Encoding.UTF8;
				_contentType = "text/html; charset=UTF-8";
				_redirectLocation = null;
				_statusDescription = null;
				_outputStream = stream;
				_output = new StreamWriter(stream, _contentEncoding);
			}
			else
			{
				_cookies = _innerResponse.Cookies;
				_headers = _innerResponse.Headers;
				_statusCode = _innerResponse.StatusCode;
				_contentEncoding = _innerResponse.ContentEncoding;
				_contentType = _innerResponse.ContentType;
				_redirectLocation = _innerResponse.RedirectLocation;
				_statusDescription = _innerResponse.StatusDescription;
				_outputStream = stream;
				_output = new StreamWriter(stream, _contentEncoding);
			}
		}

		public override HttpCookieCollection Cookies
		{
			get { return _cookies; }
		}

		public override int StatusCode
		{
			get { return _statusCode; }
			set { _statusCode = value; }
		}

		public override string StatusDescription
		{
			get { return _statusDescription; }
			set { _statusDescription = value; }
		}

		public override string RedirectLocation
		{
			get { return _redirectLocation; }
			set { _redirectLocation = value; }
		}

		public override Encoding ContentEncoding
		{
			get { return _contentEncoding; }
			set { _contentEncoding = value; }
		}

		public override string ContentType
		{
			get { return _contentType; }
			set { _contentType = value; }
		}

		public override NameValueCollection Headers
		{
			get { return _headers; }
		}

		public override TextWriter Output
		{
			get { return _output; }
		}

		public override Stream OutputStream
		{
			get { return _outputStream; }
		}

        public override string ApplyAppPathModifier(string virtualPath)
        {
			if (_innerResponse != null)
				return _innerResponse.ApplyAppPathModifier(virtualPath);
            else
                // TODO Valutare se bisogna implementare qualche logica o è sempre sufficiente restituire la virtualPath
                return virtualPath;
        }

        public override void RedirectPermanent(string url)
        {
            throw new XrcException(string.Format("Response redirection required, redirect url is '{0}'.", url));
        }

		public override void Write(string s)
		{
			_output.Write(s);
		}

		public override void Write(char ch)
		{
			_output.Write(ch);
		}

		public override void Write(char[] buffer, int index, int count)
		{
			_output.Write(buffer, index, count);
		}

		public override void Flush()
		{
			if (_isStreamOwner)
			{
				_output.Flush();
			}
			else
			{
				if (_innerResponse != null)
					_innerResponse.Flush();
			}
		}

		//public override void End()
		//{
		//    base.End();
		//}

		public override void Close()
		{
			Flush();

			if (_isStreamOwner)
			{
				_output.Close();
			}
			else
			{
				if (_innerResponse != null)
					_innerResponse.Close();
			}
		}
	}
}
