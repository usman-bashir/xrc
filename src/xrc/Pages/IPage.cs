using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using xrc.Sites;

namespace xrc.Pages
{
    public interface IPage
    {
        PageActionList Actions
        {
            get;
        }

        PageParameterList PageParameters
        {
            get;
        }

        ModuleDefinitionList Modules
        {
            get;
        }

		Uri CanonicalUrl
		{
			get;
		}

		Dictionary<string, string> UrlSegmentsParameters
		{
			get;
		}

		ISiteConfiguration SiteConfiguration { get; }

		Uri GetContentAbsoluteUrl(string contentUrlPageRelative);
	}
}
