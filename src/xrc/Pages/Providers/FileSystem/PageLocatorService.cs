﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Web;

namespace xrc.Pages.Providers.FileSystem
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

        public XrcFileResource Locate(string relativeUri)
        {
            return Locate(new Uri(relativeUri, UriKind.Relative));
        }

        public XrcFileResource Locate(Uri relativeUri)
        {
            if (relativeUri == null)
                throw new ArgumentNullException("relativeUri");
            if (relativeUri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not relative.", relativeUri));

            var urlSegmentParameters = new Dictionary<string, string>();
            XrcFolder currentFolder = Root;
            XrcFile requestFile = null;
            StringBuilder canonicalUrl = new StringBuilder("~/");

			// TODO Codice da rivedere

			UriSegmentMatchResult matchResult = null;
			string currentUrl = relativeUri.GetPath().ToLowerInvariant();
			do
			{
				currentUrl = currentUrl.TrimStart('/');
				if (string.IsNullOrEmpty(currentUrl))
					break;

				requestFile = currentFolder.GetFile(currentUrl);
				if (requestFile != null)
				{
					if (!requestFile.IsIndex)
						canonicalUrl.Append(currentUrl);
					break;
				}

				if (currentFolder.Folders.Count() == 0)
					break;

				foreach (var subFolder in currentFolder.Folders)
				{
					matchResult = subFolder.Parameter.Match(currentUrl);
					if (matchResult.Success)
					{
						canonicalUrl.AppendFormat("{0}/", matchResult.CurrentUrlPart);
						if (matchResult.IsParameter)
							urlSegmentParameters.Add(matchResult.ParameterName, matchResult.ParameterValue);

						currentFolder = subFolder;
						break;
					}
				}

				if (!matchResult.Success)
					break;

				currentUrl = matchResult.NextUrlPart;
			} while (matchResult.HasNext);

			// last segment found is not a file, so try to read the default (index) file
			if (requestFile == null && string.IsNullOrEmpty(currentUrl))
				requestFile = currentFolder.GetIndexFile();

			if (requestFile == null)
				return null; //Not found

			return new XrcFileResource(requestFile, canonicalUrl.ToString(), urlSegmentParameters);


			//if (segments.Length > 0)
			//{
			//    for (int i = 0; i < segments.Length - 1; i++)
			//    {
			//        currentFolder = currentFolder.GetFolder(segments[i]);
			//        if (currentFolder == null)
			//            return null; //Not found
			//        if (currentFolder.IsParameter)
			//            urlSegmentParameters.Add(currentFolder.ParameterName, segments[i]);

			//        canonicalUrl.AppendFormat("{0}/", segments[i]);
			//    }

			//    string lastSegment = segments.LastOrDefault();
			//    requestFile = currentFolder.GetFile(lastSegment);
			//    if (requestFile == null)
			//    {
			//        currentFolder = currentFolder.GetFolder(lastSegment);
			//        if (currentFolder == null)
			//            return null; //Not found
			//        if (currentFolder.IsParameter)
			//            urlSegmentParameters.Add(currentFolder.ParameterName, lastSegment);

			//        canonicalUrl.AppendFormat("{0}/", lastSegment);
			//    }
			//    else
			//    {
			//        if (!requestFile.IsIndex)
			//            canonicalUrl.Append(lastSegment);
			//    }
			//}

			////the last segment found is not a file, so try to read the default (index) file
			//if (requestFile == null)
			//{
			//    requestFile = currentFolder.GetIndexFile();
			//    if (requestFile == null)
			//        return null; //Not found
			//}

			//return new XrcFileResource(requestFile, canonicalUrl.ToString(), urlSegmentParameters);
        }

		//private string[] GetUriSegments(Uri relativeUri)
		//{
		//    string requestPath = relativeUri.GetPath().ToLowerInvariant();

		//    return requestPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
		//}
    }
}
