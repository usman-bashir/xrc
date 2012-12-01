using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace xrc.CustomErrors
{
	public class CustomErrorEntry : ICustomErrorEntry
    {
		readonly int _statusCode;
		readonly string _url;

		public CustomErrorEntry(int statusCode, string url)
		{
			_statusCode = statusCode;
			_url = url;
		}

		public int StatusCode
		{
			get { return _statusCode; }
		}

		public string Url
		{
			get { return _url; }
		}
	}
}
