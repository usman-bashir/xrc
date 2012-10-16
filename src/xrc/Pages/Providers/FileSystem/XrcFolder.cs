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
		private List<XrcFile> _files;
		private List<XrcFolder> _folders;
		private string _configFile;
		private ParametricUriSegment _parameter;

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
			_parameter = new ParametricUriSegment(Name);

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

		public ParametricUriSegment Parameter
		{
			get { return _parameter; }
		}

        public IEnumerable<XrcFolder> Folders
        {
			get
			{
				return _folders;
			}
        }

		public IEnumerable<XrcFile> Files
		{
			get
			{
				return _files;
			}
		}

		public XrcFile GetFile(string name)
		{
			return _files.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
		}

		public XrcFolder GetFolder(string name)
		{
			return _folders.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
		}

		public XrcFile IndexFile
        {
			get
			{
				return GetFile(XrcFileSystemHelper.INDEX_FILE);
			}
        }

		public XrcFile LayoutFile
		{
			get
			{
				return GetFile(XrcFileSystemHelper.LAYOUT_FILE);
			}
		}

		public XrcFolder SharedFolder
		{
			get
			{
				return GetFolder(XrcFileSystemHelper.SHARED_FOLDER);
			}
		}

		// TODO Da rivedere questo metodo, forse restituire un XrcFile?
		public string GetConfigFile()
		{
			return _configFile;
		}

		private void SearchFiles()
		{
			var filesStd = Directory.GetFiles(FullPath, XrcFileSystemHelper.FILE_PATTERN_STANDARD).Select(p => new XrcFile(this, Path.GetFileName(p)));
			var filesExt = Directory.GetFiles(FullPath, XrcFileSystemHelper.FILE_PATTERN_EXTENDED).Select(p => new XrcFile(this, Path.GetFileName(p)));
			_files = filesStd.Union(filesExt).ToList();
		}

		private void SearchFolders()
		{
			var folders = Directory.GetDirectories(FullPath).Select(p =>
					{
						return new XrcFolder(this, XrcFileSystemHelper.GetDirectoryName(p));
					});

            _folders = folders.ToList();
		}

		public XrcFile SearchLayout()
		{
			return SearchLayout(this);
		}

		private XrcFile SearchLayout(XrcFolder folder)
		{
			var layoutFile = folder.LayoutFile;
			if (layoutFile != null)
				return layoutFile;

			var sharedFolder = folder.SharedFolder;
			if (sharedFolder != null)
			{
				layoutFile = sharedFolder.LayoutFile;
				if (layoutFile != null)
					return layoutFile;
			}

			if (folder.Parent != null)
				return SearchLayout(folder.Parent);

			return null;
		}
	}
}
