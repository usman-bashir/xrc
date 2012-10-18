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
		public PageLocatorService(IRootPathConfig rootPathConfig)
        {
			if (!System.IO.Directory.Exists(rootPathConfig.PhysicalPath))
				throw new ApplicationException(string.Format("Path '{0}' doesn't exist.", rootPathConfig.PhysicalPath));

			RootPathConfig = rootPathConfig;
			Root = new XrcFolder(rootPathConfig);
		}

		public IRootPathConfig RootPathConfig
		{
			get;
			private set;
		}

		public XrcFolder Root
        {
            get;
            private set;
        }

		public PageLocatorResult Locate(Uri relativeUri)
        {
            if (relativeUri == null)
                throw new ArgumentNullException("relativeUri");
            if (relativeUri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not relative.", relativeUri));

			string currentUrl = relativeUri.GetPath().ToLowerInvariant();
			var urlSegmentParameters = new Dictionary<string, string>();
			XrcFolder currentFolder = Root;
			XrcFile requestFile = null;
			StringBuilder canonicalUrl = new StringBuilder("~/");

			while (!(string.IsNullOrEmpty(currentUrl) || currentUrl == "/"))
			{
				requestFile = SearchFile(urlSegmentParameters, currentFolder, canonicalUrl, ref currentUrl);
				if (requestFile != null)
					break;

				XrcFolder matchFolder = SearchFolder(urlSegmentParameters, currentFolder, canonicalUrl, ref currentUrl);
				if (matchFolder == null)
					return null; //Not found
				else
					currentFolder = matchFolder;
			}

			// last segment found is not a file, so try to read the default (index) file
			if (requestFile == null)
				requestFile = currentFolder.IndexFile;

			if (requestFile == null)
				return null; //Not found

			return new XrcFileResource(requestFile, canonicalUrl.ToString(), urlSegmentParameters);
        }

		private static XrcFile SearchFile(Dictionary<string, string> urlSegmentParameters, 
										XrcFolder currentFolder, StringBuilder canonicalUrl,
										ref string currentUrl)
		{
			foreach (var file in currentFolder.Files)
			{
				ParametricUriSegmentResult matchResult = file.Parameter.Match(currentUrl);
				if (matchResult.Success)
				{
					if (!file.IsIndex)
						canonicalUrl.Append(UriExtensions.RemoveTrailingSlash(matchResult.CurrentUrlPart));
					if (matchResult.IsParameter)
						urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

					currentUrl = matchResult.NextUrlPart;
					return file;
				}
			}

			return null;
		}

		private static XrcFolder SearchFolder(Dictionary<string, string> urlSegmentParameters, 
											XrcFolder currentFolder, StringBuilder canonicalUrl, 
											ref string currentUrl)
		{
			foreach (var subFolder in currentFolder.Folders)
			{
				ParametricUriSegmentResult matchResult = subFolder.Parameter.Match(currentUrl);
				if (matchResult.Success)
				{
					canonicalUrl.Append(UriExtensions.AppendTrailingSlash(matchResult.CurrentUrlPart));
					if (matchResult.IsParameter)
						urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

					currentUrl = matchResult.NextUrlPart;
					return subFolder;
				}
			}

			return null;
		}
    }
}
