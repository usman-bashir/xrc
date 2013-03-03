using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Web;
using System.IO;

namespace xrc.Pages.TreeStructure
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

		PageFile MatchItem(Item item, string url, UriSegmentParameterList urlSegmentParameters)
		{
			var matchResult = item.Match(url);
			if (matchResult.Success)
			{
                Item itemFound;
                if (string.IsNullOrEmpty(matchResult.NextUrlPart))
                {
                    // last segment found is not a file, so try to read the default (index) file
                    if (item.ItemType == ItemType.Directory)
                        itemFound = ((PageDirectory)item).IndexFile;
                    else if (item.ItemType == ItemType.PageFile)
                        itemFound = item;
                    else
                        throw new XrcException(string.Format("Item '{0}' is not supported.", item.ResourceLocation));
                }
                else if (item.ItemType == ItemType.Directory)
                    itemFound = MatchList((PageDirectory)item, matchResult.NextUrlPart, urlSegmentParameters);
                else
                    itemFound = null;

				if (itemFound != null && matchResult.IsParameter)
					urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

				return (PageFile)itemFound;
			}

			return null;
		}

		Item MatchList(PageDirectory directory, string url, UriSegmentParameterList urlSegmentParameters)
		{
			var validFiles= directory.Files.Where(p => p.ItemType != ItemType.ConfigFile);
            var validItems = directory.Directories.Select(p => (Item)p).Union(validFiles)
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
