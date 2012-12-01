using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

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

		XrcUrl PageUrl
		{
			get;
		}

		string ResourceLocation
		{
			get;
		}

		UriSegmentParameterList UrlSegmentsParameters
		{
			get;
		}

		string GetResourceLocation(string resourceName);

		string GetAppRelativeUrl(string resource);

		XrcUrl GetPageUrl(string page);
	}
}
