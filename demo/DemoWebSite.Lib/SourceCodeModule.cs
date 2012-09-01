using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml.Linq;
using xrc.Pages.Providers.FileSystem;

namespace DemoWebSite
{
	public class SourceCodeModule : ISourceCodeModule
    {
		private xrc.IContext _context;

		public SourceCodeModule(xrc.IContext context)
		{
			_context = context;
		}

        public string GetGitLink()
        {
			FileSystemPage fsPage = (FileSystemPage)GetOriginalContext(_context).Page;

			string file = fsPage.File.FullPath.ToLower()
								.Replace(HttpContext.Current.Request.MapPath("~").ToLower(), "")
								.Replace("\\", "/");
			return xrc.UriExtensions.Combine("https://github.com/davideicardi/xrc/blob/master/demo/DemoWebSite/", file);
        }

		private xrc.IContext GetOriginalContext(xrc.IContext context)
		{
			if (context.CallerContext == null)
				return context;
			else
				return GetOriginalContext(context.CallerContext);
		}
	}
}