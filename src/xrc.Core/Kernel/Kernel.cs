using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages;
using xrc.Configuration;
using System.IO;
using System.Reflection;
using xrc.Views;
using System.Web;
using xrc.Script;
using xrc.Sites;
using xrc.Modules;
using xrc.Pages.Providers;
using xrc.Pages.Script;

namespace xrc
{
    public class Kernel : IKernel
    {
		readonly IXrcService _xrcService;
		readonly ISiteConfigurationProviderService _siteConfigurationProvider;

		public Kernel(IXrcService xrcService, ISiteConfigurationProviderService siteConfigurationProvider)
        {
			_xrcService = xrcService;
			_siteConfigurationProvider = siteConfigurationProvider;
        }

        // TODO Check if it is possible to remove this static reference
        #region Static
        private static IKernel _current;
        public static void Init(IKernel kernel)
        {
            _current = kernel;
        }
        public static IKernel Current
        {
            get { return _current; }
        }
        #endregion

		public void Init()
		{
			Kernel.Init(this);
		}

		public bool Match(HttpContextBase httpContext)
		{
			var xrcUrl = new xrc.XrcUrl(httpContext.Request.AppRelativeCurrentExecutionFilePath);

			return _xrcService.Match(xrcUrl);
		}

		public void ProcessRequest(HttpContextBase httpContext)
		{
			var context = new Context(httpContext);
			var siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(httpContext.Request.Url);

			_xrcService.ProcessRequest(context, siteConfiguration);
		}
	}
}
