using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using xrc.Sites;

namespace xrc.Pages.Providers.Common
{
	public class PageLocatorResult
    {
		readonly XrcItem _item;
		readonly Dictionary<string, string> _urlSegmentsParameters;
		readonly XrcUrl _url;

		public PageLocatorResult(XrcItem item, Dictionary<string, string> urlSegmentsParameters)
		{
			_item = item;
			_urlSegmentsParameters = urlSegmentsParameters;
			_url = _item.GetUrl(urlSegmentsParameters);
		}

		public XrcItem Item
		{
			get { return _item; }
		}
		public Dictionary<string, string> UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}
		public XrcUrl Url
		{
			get { return _url; }
		} 
	}
}
