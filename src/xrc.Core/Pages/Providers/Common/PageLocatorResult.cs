using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Pages.Providers.Common
{
	public class PageLocatorResult
    {
		readonly XrcItem _item;
		readonly UriSegmentParameterList _urlSegmentsParameters;

		public PageLocatorResult(XrcItem item, UriSegmentParameterList urlSegmentsParameters)
		{
			_item = item;
			_urlSegmentsParameters = urlSegmentsParameters;
		}

		public XrcItem Item
		{
			get { return _item; }
		}
		public UriSegmentParameterList UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}
	}
}
