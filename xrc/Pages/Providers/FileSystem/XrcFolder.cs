using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	public class XrcFolder
	{
		private const string DEFAULT_FILE = "index";
		private const string FOLDER_CONFIG_FILE = "xrcFolder.config";
		private const string FILE_EXTENSION = ".xrc";
        private const string FILE_PATTERN = "*" + FILE_EXTENSION;

		private Dictionary<string, string> _files;
		private Dictionary<string, XrcFolder> _folders;
		private string _configFile;

		public XrcFolder(string fullPath, XrcFolder parent)
		{
			Parent = parent;
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

			string configFile = Path.Combine(fullPath, FOLDER_CONFIG_FILE);
			if (File.Exists(configFile))
				_configFile = configFile;
		}

		public XrcFolder Parent
		{
			get;
			private set;
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

		public XrcFolder DynamicFolder
		{
			get;
			private set;
		}

        public XrcFolder GetFolder(string name)
        {
            XrcFolder folder;
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

		public string GetConfigFile()
		{
			return _configFile;
		}

		private void SearchFiles()
		{
            var files = Directory.GetFiles(FullPath, FILE_PATTERN).Select(p => p.ToLowerInvariant());

            _files = files.ToDictionary(p => Path.GetFileNameWithoutExtension(p), StringComparer.OrdinalIgnoreCase);
		}

		private void SearchFolders()
		{
			var folders = Directory.GetDirectories(FullPath).Select(p => new XrcFolder(p, this));

			if (folders.Where(p => p.IsParameter).Count() > 1)
				throw new ApplicationException(string.Format("Invalid directory '{0}', cannot have more than one parametric directory.", FullPath));

			DynamicFolder = folders.SingleOrDefault(p => p.IsParameter);

            _folders = folders.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
		}
	}
}
