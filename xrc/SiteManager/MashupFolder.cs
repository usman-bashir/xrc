using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.SiteManager
{
	public class MashupFolder
	{
        private const string DEFAULT_FILE = "index";
        private const string FILE_EXTENSION = ".xrc";
        private const string FILE_PATTERN = "*" + FILE_EXTENSION;

		private Dictionary<string, string> _files;
		private Dictionary<string, MashupFolder> _folders;

		public MashupFolder(string fullPath)
		{
			FullPath = fullPath.ToLowerInvariant();
			Name = new DirectoryInfo(fullPath).Name;

			bool hasStart = Name.StartsWith("{");
			bool hasEnd = Name.EndsWith("}");
			if (hasStart && hasEnd)
			{
				IsParameter = true;
				ParameterName = Name.Substring(1, Name.Length - 2);
			}
			else if (hasStart || hasEnd)
				throw new ApplicationException(string.Format("Invalid directory name '{0}'.", fullPath));
			else
			{
				IsParameter = false;
				ParameterName = null;
			}

			SearchFolders();
			SearchFiles();
		}

        public MashupFolder GetFolder(string name)
        {
            MashupFolder folder;
            _folders.TryGetValue(name, out folder);
            if (folder != null)
                return folder;
            else
                return DynamicFolder;
        }

        public string GetFile(string name)
        {
            string file;
            _files.TryGetValue(name, out file);
            return file;
        }

        public string GetIndexFile()
        {
            string file;
            _files.TryGetValue(DEFAULT_FILE, out file);
            return file;
        }

		private void SearchFiles()
		{
            var files = Directory.GetFiles(FullPath, FILE_PATTERN).Select(p => p.ToLowerInvariant());

            _files = files.ToDictionary(p => Path.GetFileNameWithoutExtension(p), StringComparer.OrdinalIgnoreCase);
		}

		private void SearchFolders()
		{
			var folders = Directory.GetDirectories(FullPath).Select(p => new MashupFolder(p));

			if (folders.Where(p => p.IsParameter).Count() > 1)
				throw new ApplicationException(string.Format("Invalid directory '{0}', cannot have more than one parametric directory.", FullPath));

			DynamicFolder = folders.SingleOrDefault(p => p.IsParameter);

            _folders = folders.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
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

		public bool IsParameter
		{
			get;
			private set;
		}

		public string ParameterName
		{
			get;
			private set;
		}

		public MashupFolder DynamicFolder
		{
			get;
			private set;
		}
	}
}
