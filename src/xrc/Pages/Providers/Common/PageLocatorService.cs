using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Web;
using System.IO;

namespace xrc.Pages.Providers.Common
{
    public class PageLocatorService : IPageLocatorService
    {
		readonly IPageStructureService _pageStructure;

		public PageLocatorService(IPageStructureService pageStructure)
        {
			_pageStructure = pageStructure;
		}

		public PageLocatorResult Locate(string relativeUrl)
		{
			return Locate(new Uri(relativeUrl, UriKind.Relative));
		}

		public PageLocatorResult Locate(Uri relativeUrl)
        {
			if (relativeUrl == null)
                throw new ArgumentNullException("relativeUri");
			if (relativeUrl.IsAbsoluteUri)
				throw new UriFormatException(string.Format("Uri '{0}' is not relative.", relativeUrl));

			string currentUrl = relativeUrl.GetPath().ToLowerInvariant();
			var urlSegmentParameters = new Dictionary<string, string>();

			XrcItem currentItem = _pageStructure.GetRoot();
			XrcItem matchItem = null;
			StringBuilder actualVirtualUrl = new StringBuilder("~/");

			while (!(string.IsNullOrEmpty(currentUrl) || currentUrl == "/"))
			{
				matchItem = SearchItem(urlSegmentParameters, currentItem, actualVirtualUrl, ref currentUrl);
				if (matchItem == null)
					return null; //Not found
				else
					currentItem = matchItem;
			}

			// last segment found is not a file, so try to read the default (index) file
			if (matchItem == null)
				matchItem = currentItem.IndexFile;

			if (matchItem == null)
				return null; //Not found

			return new PageLocatorResult(matchItem, urlSegmentParameters, actualVirtualUrl.ToString());
        }

		private static XrcItem SearchItem(Dictionary<string, string> urlSegmentParameters, 
										XrcItem currentItem, StringBuilder canonicalUrl,
										ref string currentUrl)
		{
			foreach (var subItem in currentItem.Items)
			{
				ParametricUriSegmentResult matchResult = subItem.Match(currentUrl);
				if (matchResult.Success)
				{
					if (!subItem.IsIndex)
						canonicalUrl.Append(UriExtensions.RemoveTrailingSlash(matchResult.CurrentUrlPart));
					if (matchResult.IsParameter)
						urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

					currentUrl = matchResult.NextUrlPart;
					return subItem;
				}
			}

			return null;
		}
    }
}
