using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
	public class XrcUrl
	{
		readonly string _url;
		public XrcUrl(string pageVirtualurl)
		{
			if (pageVirtualurl == null)
				throw new ArgumentNullException("url");

			if (!pageVirtualurl.StartsWith("~/"))
				throw new XrcException(string.Format("Invalid xrc url format: '{0}'.", pageVirtualurl));

			_url = pageVirtualurl.ToLowerInvariant();
		}

		public string Path
		{
			get { throw new NotImplementedException(); }
		}

		public string Query
		{
			get { throw new NotImplementedException(); }
		}
	}
}
