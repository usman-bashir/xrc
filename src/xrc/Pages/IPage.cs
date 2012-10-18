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

		string LogicalVirtualPath
		{
			get;
		}

		string PhysicalVirtualPath
		{
			get;
		}

		Dictionary<string, string> UrlSegmentsParameters
		{
			get;
		}

		ISiteConfiguration SiteConfiguration { get; }

		string ContentVirtualUrl(string relativeUrl, ContentUrlMode mode);

		bool IsCanonicalUrl(Uri url);
	}

	public enum ContentUrlMode
	{
		Logical,
		Physical
	}
}
