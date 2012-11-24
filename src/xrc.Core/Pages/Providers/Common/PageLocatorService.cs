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

		public PageLocatorResult Locate(XrcUrl url)
        {
			if (url == null)
				throw new ArgumentNullException("url");

			var urlSegmentParameters = new UriSegmentParameterList();

			var matchItem = MatchItem(_pageStructure.GetRoot(), url.Path, urlSegmentParameters);

			if (matchItem == null)
				return null;

			return new PageLocatorResult(matchItem, urlSegmentParameters);
        }

		XrcItem MatchItem(XrcItem item, string url, UriSegmentParameterList urlSegmentParameters)
		{
			var matchResult = item.Match(url);
			if (matchResult.Success)
			{
				XrcItem itemFound;
				if (string.IsNullOrEmpty(matchResult.NextUrlPart))
				{
					// last segment found is not a file, so try to read the default (index) file
					if (item.ItemType == XrcItemType.Directory)
						itemFound = item.IndexFile;
					else if (item.ItemType == XrcItemType.XrcFile)
						itemFound = item;
					else
						throw new XrcException(string.Format("Item '{0}' is not supported.", item.ResourceLocation));
				}
				else
					itemFound = MatchList(item.Items, matchResult.NextUrlPart, urlSegmentParameters);

				if (itemFound != null && matchResult.IsParameter)
					urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

				return itemFound;
			}

			return null;
		}

		XrcItem MatchList(XrcItemList list, string url, UriSegmentParameterList urlSegmentParameters)
		{
			var validItems = list.Where(p => p.ItemType != XrcItemType.ConfigFile)
								.OrderBy(p => p.Priority);

			foreach (var item in validItems)
			{
				var match = MatchItem(item, url, urlSegmentParameters);
				if (match != null)
					return match;
			}

			return null;
		}
    }
}
