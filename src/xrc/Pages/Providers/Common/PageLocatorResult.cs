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

		public PageLocatorResult(XrcItem item, Dictionary<string, string> urlSegmentsParameters)
		{
			_item = item;
			_urlSegmentsParameters = urlSegmentsParameters;
		}

		public XrcItem Item
		{
			get { return _item; }
		}
		public Dictionary<string, string> UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}
	}
}
