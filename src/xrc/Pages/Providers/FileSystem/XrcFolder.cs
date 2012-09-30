using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using xrc.Configuration;

namespace xrc.Pages.Providers.FileSystem
{
	//TODO Rivedere il codice di XrcFolder e XrcFile, probabilmente spostare parte del codice di questa classe in PageLocatorService
	// dovrei probabilmente rimuovere la dipendenza al FileSystem

	public class XrcFolder
	{
		private Dictionary<string, XrcFile> _files;
		private Dictionary<string, XrcFolder> _folders;
		private string _configFile;
		private UriSegmentParameter _parameter;

		public XrcFolder(IRootPathConfig rootPathConfig)
		{
			if (rootPathConfig == null)
				throw new ArgumentNullException("rootPathConfig");

			var directoryName = XrcFileSystemHelper.GetDirectoryName(rootPathConfig.PhysicalPath);
			Name = XrcFileSystemHelper.GetDirectoryLogicalName(directoryName);
			Parent = null;
			FullPath = rootPathConfig.PhysicalPath.ToLowerInvariant();
			VirtualPath = UriExtensions.AppendTrailingSlash(rootPathConfig.VirtualPath).ToLowerInvariant();
			FullName = "~";

			Init();
		}

		public XrcFolder(XrcFolder parent, string directoryName)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (string.IsNullOrEmpty(directoryName))
				throw new ArgumentNullException("directoryName");

			Name = XrcFileSystemHelper.GetDirectoryLogicalName(directoryName);
			Parent = parent;
			FullPath = Path.Combine(parent.FullPath, Name);
			VirtualPath = UriExtensions.AppendTrailingSlash(UriExtensions.Combine(parent.VirtualPath, Name));
			FullName = UriExtensions.Combine(parent.FullName, Name);

			Init();
		}

		private void Init()
		{
			_parameter = new UriSegmentParameter(Name);

			SearchFolders();
			SearchFiles();

			string configFile = Path.Combine(FullPath, XrcFileSystemHelper.FOLDER_CONFIG_FILE);
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

		public string VirtualPath
		{
			get;
			private set;
		}

		public string FullName
		{
			get;
			private set;
		}

		public UriSegmentParameter Parameter
		{
			get { return _parameter; }
		}

		public XrcFolder GetFolder(string name)
		{
			XrcFolder folder;
			_folders.TryGetValue(name, out folder);
			return folder;
		}

        public IEnumerable<XrcFolder> Folders
        {
			get
			{
				return _folders.Values;
			}
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
			var files = Directory.GetFiles(FullPath, XrcFileSystemHelper.FILE_PATTERN).Select(p => new XrcFile(this, Path.GetFileName(p)));

			var dictionary = new Dictionary<string, XrcFile>(StringComparer.OrdinalIgnoreCase);
			foreach (var f in files)
			{
				if (dictionary.ContainsKey(f.Name))
					throw new XrcException(string.Format("A page with the same name is already specified for file '{0}'.", f.FullPath));

				dictionary.Add(f.Name, f);
			}

			_files = dictionary;
		}

		private void SearchFolders()
		{
			var folders = Directory.GetDirectories(FullPath).Select(p =>
					{
						return new XrcFolder(this, XrcFileSystemHelper.GetDirectoryName(p));
					});

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
