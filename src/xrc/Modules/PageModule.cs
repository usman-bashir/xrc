using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml.Linq;
using xrc.Pages.Providers;
using xrc.Pages;

namespace xrc.Modules
{
    public class PageModule : IPageModule
    {
        IContext _context;
		IPageProviderService _pageProvider;

		public PageModule(IContext context, IPageProviderService pageProvider)
        {
            _context = context;
			_pageProvider = pageProvider;
        }

		public Pages.IPage GetCurrent()
		{
			return _context.Page;
		}

		public Pages.IPage Get(string url)
		{
			XrcUrl xrcUrl = new XrcUrl(_context.Page.GetContentVirtualUrl(url));

			return _pageProvider.GetPage(xrcUrl, _context.Page.SiteConfiguration);
		}

		public Pages.IPage GetInitiator()
		{
			return _context.GetInitiatorContext().Page;
		}

		public object GetPageParameter(string url, string parameter)
		{
			var page = Get(url);
			if (page == null)
				throw new XrcException(string.Format("Page '{0}' not found.", url));

			PageParameter pageParam;
			if (!page.PageParameters.TryGetValue(parameter, out pageParam))
				return null;

			if (pageParam.Value.Expression != null)
				throw new XrcException(string.Format("Expression parameters are not supported. Parameter '{0}', page '{1}'.", parameter, url));

			return pageParam.Value.Value;
		}

		/// <summary>
		/// Same as GetPageParameter but in case of null returns an empty string because null values are not supported inside an xslt.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public object GetPageParameterXslt(string url, string parameter)
		{
			return GetPageParameter(url, parameter) ?? string.Empty;
		}
	}
}
