using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
	public class XrcUrl
	{
		readonly string _path;
		readonly string _query;
		readonly string _appRelaviteUrl;

		public XrcUrl(Uri appRelativeUrl)
			: this(appRelativeUrl.ToString())
		{

		}

		public XrcUrl(string appRelativeUrl)
		{
			if (appRelativeUrl == null)
				throw new ArgumentNullException("appRelativeUrl");

			if (!UriExtensions.IsAppRelativeVirtualUrl(appRelativeUrl))
				throw new XrcException(string.Format("Invalid xrc url format: '{0}'.", appRelativeUrl));

			appRelativeUrl = appRelativeUrl.ToLowerInvariant();

			_path = UriExtensions.GetPath(appRelativeUrl);
			_query = UriExtensions.GetQuery(appRelativeUrl);
			_appRelaviteUrl = appRelativeUrl;
		}

		public string AppRelaviteUrl
		{
			get { return _appRelaviteUrl; }
		}

		public string Path
		{
			get { return _path; }
		}

		public string Query
		{
			get { return _query; }
		}

		public XrcUrl AppendTrailingSlash()
		{
			return new XrcUrl(UriExtensions.AppendTrailingSlash(AppRelaviteUrl));
		}

		public XrcUrl Append(string segment)
		{
			return new XrcUrl(UriExtensions.Combine(AppRelaviteUrl, segment));
		}

		public override string ToString()
		{
			return _appRelaviteUrl;
		}
	}
}
