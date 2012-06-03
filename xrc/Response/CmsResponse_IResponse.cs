using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.IO;

namespace d3cms
{
	public class CmsResponse : IResponse, IDisposable
	{
		private HttpCookieCollection _cookies = new HttpCookieCollection();
		private HttpStatusCode _statusCode = HttpStatusCode.OK;
		private Encoding _contentEncoding;
		private string _contentType;
		private NameValueCollection _headers = new NameValueCollection();
		private string _redirectLocation;
		private string _statusDescription;
		private TextWriter _output;
		private Stream _outputStream;

		public CmsResponse(Stream stream, Encoding encoding = null)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			if (encoding == null)
				encoding = Encoding.UTF8;

			_contentEncoding = encoding;
			_outputStream = stream;
			_output = new StreamWriter(stream, _contentEncoding);
		}

		public HttpCookieCollection Cookies
		{
			get { return _cookies; }
		}

		public HttpStatusCode StatusCode
		{
			get { return _statusCode; }
			set { _statusCode = value; }
		}

		public string StatusDescription
		{
			get { return _statusDescription; }
			set { _statusDescription = value; }
		}

		public string RedirectLocation
		{
			get { return _redirectLocation; }
			set { _redirectLocation = value; }
		}

		public Encoding ContentEncoding
		{
			get { return _contentEncoding; }
		}

		public string ContentType
		{
			get { return _contentType; }
			set { _contentType = value; }
		}

		public NameValueCollection Headers
		{
			get { return _headers; }
		}

		public TextWriter Output
		{
			get { return _output; }
		}

		public Stream OutputStream
		{
			get { return _outputStream; }
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

					// TODO Check it is right.
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
