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

	public class XrcResponse : HttpResponseBase, IDisposable
	{
		private HttpCookieCollection _cookies = new HttpCookieCollection();
		private int _statusCode = (int)HttpStatusCode.OK;
		private Encoding _contentEncoding;
		private string _contentType;
		private NameValueCollection _headers = new NameValueCollection();
		private string _redirectLocation;
		private string _statusDescription;
		private TextWriter _output;
		private Stream _outputStream;

        private HttpResponseBase _parentResponse;

		public XrcResponse(Stream stream, Encoding encoding = null, HttpResponseBase parentResponse = null)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			if (encoding == null)
				encoding = Encoding.UTF8;

			_contentEncoding = encoding;
			_outputStream = stream;
			_output = new StreamWriter(stream, _contentEncoding);
            _parentResponse = parentResponse;
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
            if (_parentResponse != null)
                return _parentResponse.ApplyAppPathModifier(virtualPath);
            else
                // TODO Valutare se bisogna implementare qualche logica o è sempre sufficiente restituire la virtualPath
                return virtualPath;
        }

        public override void RedirectPermanent(string url)
        {
            throw new XrcException(string.Format("Response redirection required, redirect url is '{0}'.", url));
        }

		#region IDisposable
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					if (_output != null)
					{
						_output.Flush();
					}

					// TODO Check it is right. Some classes like StreamReader seems that close the internal stream.
					// Note:
					// I should not dispose this stream because it is an input of this class.
					// It is caller responsability to dispose it.

					//if (_output != null)
					//{
					//    _output.Dispose();
					//    _output = null;
					//}
					//if (_outputStream != null)
					//{
					//    _outputStream.Dispose();
					//    _outputStream = null;
					//}
				}

				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
