﻿using System;
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
using xrc.Modules;
using xrc.Pages.Providers;
using xrc.Pages.Script;

namespace xrc
{
    public class Kernel : IKernel
    {
		readonly IXrcService _xrcService;
		readonly IHostingConfig _hostingConfig;

		public Kernel(IXrcService xrcService, IHostingConfig hostingConfig)
        {
			_xrcService = xrcService;
			_hostingConfig = hostingConfig;
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
			var relativeUrl = httpContext.Request.RawUrl;
			var appRelativeUrl = _hostingConfig.RelativeUrlToAppRelativeUrl(new Uri(relativeUrl, UriKind.Relative));

			var xrcUrl = new xrc.XrcUrl(appRelativeUrl);

			return Match(xrcUrl);
		}

		public void ProcessRequest(HttpContextBase httpContext)
		{
			var context = new Context(httpContext);

			ProcessRequest(context);
		}

		public bool Match(XrcUrl url)
		{
			return _xrcService.Match(url);
		}

		public void ProcessRequest(IContext context)
		{
			_xrcService.ProcessRequest(context);
		}
	}
}
