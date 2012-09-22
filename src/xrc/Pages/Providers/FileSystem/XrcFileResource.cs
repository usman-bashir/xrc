using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
    public class XrcFileResource
    {
		public XrcFileResource(XrcFile file, string canonicalVirtualUrl, string virtualPath, Dictionary<string, string> urlSegmentsParameters)
        {
			if (file == null)
				throw new ArgumentNullException("file");
			if (string.IsNullOrWhiteSpace(canonicalVirtualUrl))
				throw new ArgumentNullException("canonicalVirtualUrl");
			if (urlSegmentsParameters == null)
				throw new ArgumentNullException("urlSegmentsParameters");

			File = file;
            UrlSegmentsParameters = urlSegmentsParameters;
			CanonicalVirtualUrl = canonicalVirtualUrl;
			VirtualPath = virtualPath;
		}

		public XrcFile File
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

		/// <summary>
		/// Physical virtual Url of the current folder.
		/// </summary>
		public string VirtualPath
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
                return Path.GetDirectoryName(File.FullPath);
            }
        }
    }
}
