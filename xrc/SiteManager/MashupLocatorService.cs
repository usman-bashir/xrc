using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Web;

namespace xrc.SiteManager
{
    public class MashupLocatorService : IMashupLocatorService
    {
        public MashupLocatorService(WorkingPath workingPath)
        {
            if (!System.IO.Directory.Exists(workingPath))
                throw new ApplicationException(string.Format("Path '{0}' doesn't exist.", workingPath));

            Root = new MashupFolder(workingPath);
		}

		public MashupFolder Root
        {
            get;
            private set;
        }

        public MashupFile Locate(string relativeUri)
        {
            return Locate(new Uri(relativeUri, UriKind.Relative));
        }

        public MashupFile Locate(Uri relativeUri)
        {
            if (relativeUri == null)
                throw new ArgumentNullException("relativeUri");
            if (relativeUri.IsAbsoluteUri)
                throw new UriFormatException(string.Format("Uri '{0}' is not relative.", relativeUri));

            var urlSegmentParameters = new Dictionary<string, string>();
            string[] segments = GetUriSegments(relativeUri);
            MashupFolder currentFolder = Root;
            string requestFile = null;

            if (segments.Length > 0)
            {
                for (int i = 0; i < segments.Length - 1; i++)
                {
                    currentFolder = currentFolder.GetFolder(segments[i]);
                    if (currentFolder == null)
                        return null; //Not found
                    if (currentFolder.IsParameter)
                        urlSegmentParameters.Add(currentFolder.ParameterName, segments[i].ToLowerInvariant());
                }

                string lastSegment = segments.LastOrDefault();
                requestFile = currentFolder.GetFile(lastSegment);
                if (requestFile == null)
                {
                    currentFolder = currentFolder.GetFolder(lastSegment);
                    if (currentFolder == null)
                        return null; //Not found
                    if (currentFolder.IsParameter)
                        urlSegmentParameters.Add(currentFolder.ParameterName, lastSegment.ToLowerInvariant());
                }
            }

			//the last segment found is not a file, so try to read the default (index) file
            if (requestFile == null)
            {
                requestFile = currentFolder.GetIndexFile();
                if (requestFile == null)
                    return null; //Not found
            }

            return new MashupFile(requestFile, urlSegmentParameters);
        }

        private string[] GetUriSegments(Uri relativeUri)
        {
            string requestPath = relativeUri.GetPath();

            return requestPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
