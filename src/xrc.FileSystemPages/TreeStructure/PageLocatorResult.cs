using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Pages.TreeStructure
{
	public class PageLocatorResult
    {
        readonly PageFile _page;
		readonly UriSegmentParameterList _urlSegmentsParameters;

        public PageLocatorResult(PageFile page, UriSegmentParameterList urlSegmentsParameters)
		{
            _page = page;
			_urlSegmentsParameters = urlSegmentsParameters;
		}

        public PageFile Page
		{
			get { return _page; }
		}
		public UriSegmentParameterList UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}
	}
}
