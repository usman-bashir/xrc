using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml.Linq;

namespace DemoWebSite
{
	public class SourceCodeModule : xrc.Modules.IModule
    {
		private xrc.IContext _context;

		public SourceCodeModule(xrc.IContext context)
		{
			_context = context;
		}

		private xrc.IContext GetOriginalContext(xrc.IContext context)
		{
			if (context.CallerContext == null)
				return context;
			else
				return GetOriginalContext(context.CallerContext);
		}

        public string GetGitLink()
        {
			string file = GetOriginalContext(_context).File.FullPath.ToLower()
								.Replace(HttpContext.Current.Request.MapPath("~").ToLower(), "")
								.Replace("\\", "/");
			return xrc.UriExtensions.Combine("https://github.com/davideicardi/xrc/blob/master/DemoWebSite/", file);
        }
    }
}