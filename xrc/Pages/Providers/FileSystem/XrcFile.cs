using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
    public class XrcFile
    {
		public XrcFile(string xrcFile, XrcFolder parent, string canonicalVirtualUrl, Dictionary<string, string> urlSegmentsParameters)
        {
			if (string.IsNullOrWhiteSpace(xrcFile))
				throw new ArgumentNullException("xrcFile");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (string.IsNullOrWhiteSpace(canonicalVirtualUrl))
				throw new ArgumentNullException("canonicalVirtualUrl");
			if (urlSegmentsParameters == null)
				throw new ArgumentNullException("urlSegmentsParameters");

			Parent = parent;
			FullPath = xrcFile.ToLowerInvariant();
			Name = Path.GetFileNameWithoutExtension(FullPath).ToLowerInvariant();
            UrlSegmentsParameters = urlSegmentsParameters;
			CanonicalVirtualUrl = canonicalVirtualUrl;
		}

		public string Name
		{
			get;
			private set;
		}

		public string FullPath
		{
			get;
			private set;
		}

		public XrcFolder Parent
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the canonical url for the specified file.
        /// A canonical url is always lower case, doesn't include the index page but just append a slash at the end.
        /// Example: ~/folder1/, ~/folder1/page1
        /// 
        /// </summary>
        public string CanonicalVirtualUrl
        {
            get;
            private set;
        }

        public Dictionary<string, string> UrlSegmentsParameters
        {
            get;
            private set;
        }

        public string WorkingPath 
        {
            get
            {
                return Path.GetDirectoryName(FullPath);
            }
        }
    }
}
