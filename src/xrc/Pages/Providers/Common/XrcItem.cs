using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common
{
	// TODO Valutare se fare più classi

	public sealed class XrcItem
    {
		readonly XrcItem _parent;
		readonly XrcItemType _itemType;
		readonly string _id;
		readonly string _fileName;

		readonly string _name;
		readonly string _virtualPath;
		readonly string _fileExtension;
		readonly List<XrcItem> _items = new List<XrcItem>();
		readonly ParametricUriSegment _parametricSegment;

		private XrcItem(XrcItem parent, XrcItemType itemType, string id, string fileName,
						string name, string virtualPath, ParametricUriSegment parametricSegment,
						string fileExtension)
		{
			_parent = parent;
			_id = id;
			_fileName = fileName;
			_itemType = itemType;
			_name = name;
			_virtualPath = virtualPath;
			_parametricSegment = parametricSegment;
			_fileExtension = fileExtension;
		}

		public static XrcItem NewRoot(string id)
		{
			return new XrcItem(null, XrcItemType.Directory, id, null, "~", "~/", null, null);
		}
		public static XrcItem NewDirectory(XrcItem parent, string id, string directoryName)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			string name = GetDirectoryLogicalName(directoryName);
			string virtualPath = UriExtensions.AppendTrailingSlash(UriExtensions.Combine(parent.VirtualPath, name));
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(parent, XrcItemType.Directory, id, directoryName, name, virtualPath, parametricSegment, null);
		}
		public static XrcItem NewXrcFile(XrcItem parent, string id, string fileName)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			string name = GetFileLogicalName(fileName);
			string virtualPath = UriExtensions.Combine(parent.VirtualPath, name);
			string fileExtension = GetFileExtension(fileName);
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(parent, XrcItemType.XrcFile, id, fileName, name, virtualPath, parametricSegment, fileExtension);
		}
		public static XrcItem NewConfigFile(XrcItem parent, string id, string fileName)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			string name = GetConfigLogicalName(fileName);
			string virtualPath = UriExtensions.Combine(parent.VirtualPath, name);
			string fileExtension = GetFileExtension(fileName);

			return new XrcItem(parent, XrcItemType.ConfigFile, id, fileName, name, virtualPath, null, fileExtension);
		}

		public string FileName
		{
			get { return _fileName; }
		}

		public string Name
		{
			get { return _name; }
		}

		public string VirtualPath
		{
			get { return _virtualPath; }
		}

		public string Id
		{
			get { return _id; }
		}

		public XrcItem Parent
		{
			get { return _parent;}
		}

		public XrcItem IndexFile
		{
			get
			{
				return GetItem(XRC_INDEX_FILE, XrcItemType.XrcFile);
			}
		}

		public XrcItem LayoutFile
		{
			get
			{
				if (LocalLayoutFile != null)
					return LocalLayoutFile;
				else if (SharedFolder != null && SharedFolder.LocalLayoutFile != null)
					return SharedFolder.LocalLayoutFile;
				else if (_parent != null)
					return _parent.LayoutFile;
				else
					return null;
			}
		}

		public XrcItem LocalLayoutFile
		{
			get { return GetItem(XRC_LAYOUT_FILE, XrcItemType.XrcFile); }
		}

		public XrcItem SharedFolder
		{
			get
			{
				return GetItem(XRC_SHARED_DIRECTORY, XrcItemType.Directory);
			}
		}

		public XrcItem ConfigFile
		{
			get
			{
				return GetItem(XRC_DIRECTORY_CONFIG_FILE, XrcItemType.ConfigFile);
			}
		}

		public bool IsRoot
		{
			get
			{
				return _parent == null;
			}
		}

		public bool IsIndex
		{
			get 
			{
				return string.Equals(_name, XRC_INDEX_FILE, StringComparison.InvariantCultureIgnoreCase)
					&& _itemType == XrcItemType.XrcFile; 
			}
		}

		public bool IsSlot
		{
			get 
			{ 
				return _name.StartsWith("_")
					&& _itemType == XrcItemType.XrcFile; 
			}
		}

		public XrcItemType ItemType
		{
			get { return _itemType; }
		}

		public List<XrcItem> Items
		{
			get
			{
				return _items;
			}
		}

		public XrcItem GetItem(string name, XrcItemType itemType)
		{
			return _items.FirstOrDefault(p => p.ItemType == itemType
											&& string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
		}

		public ParametricUriSegmentResult Match(string url)
		{
			if (_parametricSegment == null)
				throw new XrcException(string.Format("Match not supported on '{0}'.", ItemType));

			return _parametricSegment.Match(url);
		}


		#region Static and constants
		// TODO Valutare se spostare

		public const string XRC_INDEX_FILE = "index";
		public const string XRC_LAYOUT_FILE = "_layout";
		public const string XRC_SHARED_DIRECTORY = "shared";
		public const string XRC_DIRECTORY_CONFIG_FILE = "xrc.config";
		//public const string XRC_FILE_EXTENSION = ".xrc";
		//public const string XRC_FILE_PATTERN_STANDARD = "*.xrc";
		//public const string XRC_FILE_PATTERN_EXTENDED = "*.xrc.*";

		public static string GetFileLogicalName(string fileName)
		{
			throw new NotImplementedException();
		}

		public static string GetDirectoryLogicalName(string directoryName)
		{
			throw new NotImplementedException();
		}

		public static string GetConfigLogicalName(string configName)
		{
			throw new NotImplementedException();
		}
		public static string GetFileExtension(string fileName)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
