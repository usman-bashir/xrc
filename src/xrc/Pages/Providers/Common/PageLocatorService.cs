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

			var urlSegmentParameters = new Dictionary<string, string>();

			var matchItem = MatchItem(_pageStructure.GetRoot(), url.Path, urlSegmentParameters);

			if (matchItem == null)
				return null;

			return new PageLocatorResult(matchItem, urlSegmentParameters);
        }

		XrcItem MatchItem(XrcItem item, string url, Dictionary<string, string> urlSegmentParameters)
		{
			var matchResult = item.Match(url);
			if (matchResult.Success)
			{
				if (matchResult.IsParameter)
					urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

				if (string.IsNullOrEmpty(matchResult.NextUrlPart))
				{
					// last segment found is not a file, so try to read the default (index) file
					if (item.ItemType == XrcItemType.Directory)
						return item.IndexFile;
					else if (item.ItemType == XrcItemType.XrcFile)
						return item;
					else
						throw new XrcException(string.Format("Item '{0}' is not supported.", item.ResourceLocation));
				}
				else
					return MatchList(item.Items, matchResult.NextUrlPart, urlSegmentParameters);
			}

			return null;
		}

		XrcItem MatchList(XrcItemList list, string url, Dictionary<string, string> urlSegmentParameters)
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
