using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	//TODO Rivedere il codice di XrcFolder e XrcFile, probabilmente spostare parte del codice di questa classe in PageLocatorService

	public class XrcFolder
	{
		private Dictionary<string, XrcFile> _files;
		private Dictionary<string, XrcFolder> _folders;
		private string _configFile;

		public XrcFolder(string fullPath, XrcFolder parent)
		{
			Parent = parent;
			FullPath = fullPath.ToLowerInvariant();
			Name = XrcFileSystemHelper.GetDirectoryName(fullPath);
			if (parent != null)
				FullName = UriExtensions.Combine(parent.FullName, Name);
			else
				FullName = "~";

			ParameterName = XrcFileSystemHelper.GetDirectoryParameterName(Name);

			SearchFolders();
			SearchFiles();

			string configFile = Path.Combine(fullPath, XrcFileSystemHelper.FOLDER_CONFIG_FILE);
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

		public string FullName
		{
			get;
			private set;
		}

		public bool IsParameter
		{
			get { return ParameterName != null; }
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

		public XrcFile GetFile(string name)
        {
			XrcFile file;
            _files.TryGetValue(name, out file);
            return file;
        }

		public XrcFile GetIndexFile()
        {
			XrcFile file;
			_files.TryGetValue(XrcFileSystemHelper.INDEX_FILE, out file);
            return file;
        }

		public XrcFile GetLayoutFile()
		{
			XrcFile file;
			_files.TryGetValue(XrcFileSystemHelper.LAYOUT_FILE, out file);
			return file;
		}

		public XrcFolder GetSharedFolder()
		{
			XrcFolder folder;
			_folders.TryGetValue(XrcFileSystemHelper.SHARED_FOLDER, out folder);
			return folder;
		}

		public string GetConfigFile()
		{
			return _configFile;
		}

		private void SearchFiles()
		{
			var files = Directory.GetFiles(FullPath, XrcFileSystemHelper.FILE_PATTERN).Select(p => new XrcFile(p, this));

            _files = files.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
		}

		private void SearchFolders()
		{
			var folders = Directory.GetDirectories(FullPath).Select(p => new XrcFolder(p, this));

			if (folders.Where(p => p.IsParameter).Count() > 1)
				throw new ApplicationException(string.Format("Invalid directory '{0}', cannot have more than one parametric directory.", FullPath));

			DynamicFolder = folders.SingleOrDefault(p => p.IsParameter);

            _folders = folders.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
		}

		public XrcFile SearchLayout()
		{
			return SearchLayout(this);
		}

		private XrcFile SearchLayout(XrcFolder folder)
		{
			var layoutFile = folder.GetLayoutFile();
			if (layoutFile != null)
				return layoutFile;

			var sharedFolder = folder.GetSharedFolder();
			if (sharedFolder != null)
			{
				layoutFile = sharedFolder.GetLayoutFile();
				if (layoutFile != null)
					return layoutFile;
			}

			if (folder.Parent != null)
				return SearchLayout(folder.Parent);

			return null;
		}
	}
}
