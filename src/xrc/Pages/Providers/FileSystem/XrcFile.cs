using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
    public class XrcFile
    {
		public XrcFile(string fullPath, XrcFolder parent)
        {
			if (string.IsNullOrWhiteSpace(fullPath))
				throw new ArgumentNullException("fullPath");
			if (parent == null)
				throw new ArgumentNullException("parent");

			Parent = parent;
			FullPath = fullPath.ToLowerInvariant();

			Name = XrcFileSystemHelper.GetFileName(FullPath);
			Extension = XrcFileSystemHelper.GetFileExtension(FullPath);
		}

		public string Name
		{
			get;
			private set;
		}

		public string Extension
		{
			get;
			private set;
		}

		public string FullPath
		{
			get;
			private set;
		}

		public string FileName
		{
			get { return Path.GetFileName(FullPath); }
		}

		public XrcFolder Parent
		{
			get;
			private set;
		}

		public bool IsIndex
		{
			get { return string.Equals(Name, XrcFileSystemHelper.INDEX_FILE, StringComparison.InvariantCultureIgnoreCase); }
		}
    }
}
