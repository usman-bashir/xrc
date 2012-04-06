using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.SiteManager
{
    public class MashupFile
    {
        public MashupFile(string xrcFile, Dictionary<string, string> urlSegmentsParameters)
        {
			FullPath = xrcFile.ToLowerInvariant();
			Name = Path.GetFileNameWithoutExtension(FullPath).ToLowerInvariant();
            UrlSegmentsParameters = urlSegmentsParameters;
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
