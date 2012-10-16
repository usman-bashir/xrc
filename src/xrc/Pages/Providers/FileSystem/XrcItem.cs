using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	public enum XrcItemType
	{
		Xrc,
		Directory,
		Config
	}

    public class XrcItem
    {
		readonly XrcItem _parent;
		readonly string _fullPath;
		readonly XrcItemType _itemType;

		readonly string _name;
		readonly string _fullName;
		readonly string _fileExtension;
		readonly XrcItem _indexFile;
		readonly XrcItem _localLayoutFile;
		readonly XrcItem _sharedFolder;
		readonly XrcItem _configFile;
		readonly bool _isIndex;
		readonly bool _isSlot;
		readonly List<XrcItem> _items = new List<XrcItem>();
		readonly ParametricUriSegment _parametricSegment;

		public XrcItem(XrcItem parent, string fullPath, XrcItemType itemType)
		{
			_parent = parent;
			_fullPath = fullPath;
			_itemType = itemType;

			string physicalName = Path.GetFileName(_fullPath);
			if (_itemType == XrcItemType.Directory)
			{
				_name = XrcFileSystemHelper.GetDirectoryLogicalName(physicalName);
				if (parent != null)
					_fullName = UriExtensions.Combine(parent.FullName, _name);
				else
					_fullName = "~";

				_indexFile = GetItem(XrcFileSystemHelper.INDEX_FILE, XrcItemType.Xrc);
				_localLayoutFile = GetItem(XrcFileSystemHelper.LAYOUT_FILE, XrcItemType.Xrc);
				_sharedFolder = GetItem(XrcFileSystemHelper.SHARED_FOLDER, XrcItemType.Directory);
				_configFile = GetItem(XrcFileSystemHelper.FOLDER_CONFIG_FILE, XrcItemType.Config);
				_parametricSegment = new ParametricUriSegment(_name);
			}
			else if (_itemType == XrcItemType.Xrc)
			{
				if (parent == null)
					throw new ArgumentNullException("parent");

				_name = XrcFileSystemHelper.GetFileLogicalName(physicalName);
				_fullName = UriExtensions.Combine(parent.FullName, _name);
				_fileExtension = XrcFileSystemHelper.GetFileExtension(physicalName);
				_isIndex = string.Equals(_name, XrcFileSystemHelper.INDEX_FILE, StringComparison.InvariantCultureIgnoreCase);
				_isSlot = _name.StartsWith("_");
				_parametricSegment = new ParametricUriSegment(_name);
			}
			else if (_itemType == XrcItemType.Config)
			{
				if (parent == null)
					throw new ArgumentNullException("parent");

				_name = XrcFileSystemHelper.GetConfigLogicalName(physicalName);
				_fullName = UriExtensions.Combine(parent.FullName, _name);
				_fileExtension = XrcFileSystemHelper.GetFileExtension(physicalName);
			}
			else
				throw new XrcException(string.Format("XrcItemType '{0}' not supported.", _itemType));
		}

		public string Name
		{
			get { return _name; }
		}

		public string FullName
		{
			get { return _fullName; }
		}

		public string FullPath
		{
			get { return _fullPath; }
		}

		public XrcItem Parent
		{
			get { return _parent;}
		}

		public XrcItem IndexFile
		{
			get
			{
				return _indexFile;
			}
		}

		public XrcItem LayoutFile
		{
			get
			{
				if (_localLayoutFile != null)
					return _localLayoutFile;
				else if (_sharedFolder != null && _sharedFolder.LocalLayoutFile != null)
					return _sharedFolder.LocalLayoutFile;
				else if (_parent != null)
					return _parent.LayoutFile;
				else
					return null;
			}
		}

		public XrcItem LocalLayoutFile
		{
			get { return _localLayoutFile; }
		}

		public XrcItem SharedFolder
		{
			get
			{
				return _sharedFolder;
			}
		}

		public XrcItem ConfigFile
		{
			get
			{
				return _configFile;
			}
		}

		public bool IsIndex
		{
			get { return _isIndex; }
		}

		public bool IsSlot
		{
			get { return _isSlot; }
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
	}
}
