using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common
{
    public class XrcItem
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

		public XrcItem(XrcItem parent, XrcItemType itemType, string id, string fileName)
		{
			_parent = parent;
			_id = id;
			_fileName = fileName;
			_itemType = itemType;

			if (_itemType == XrcItemType.Directory)
			{
				_name = GetDirectoryLogicalName(_fileName);
				if (parent != null)
					_virtualPath = UriExtensions.Combine(parent.VirtualPath, _name);
				else
					_virtualPath = "~/";
				_virtualPath = UriExtensions.AppendTrailingSlash(_virtualPath);

				_parametricSegment = new ParametricUriSegment(_name);
			}
			else if (_itemType == XrcItemType.XrcFile)
			{
				if (parent == null)
					throw new ArgumentNullException("parent");

				_name = GetFileLogicalName(_fileName);
				_virtualPath = UriExtensions.Combine(parent.VirtualPath, _name);
				_fileExtension = GetFileExtension(_fileName);
				_parametricSegment = new ParametricUriSegment(_name);
			}
			else if (_itemType == XrcItemType.ConfigFile)
			{
				if (parent == null)
					throw new ArgumentNullException("parent");

				_name = GetConfigLogicalName(_fileName);
				_virtualPath = UriExtensions.Combine(parent.VirtualPath, _name);
				_fileExtension = GetFileExtension(_fileName);
			}
			else
				throw new XrcException(string.Format("XrcItemType '{0}' not supported.", _itemType));
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

		public bool IsIndex
		{
			get 
			{ 
				return string.Equals(_name, XrcFileSystemHelper.INDEX_FILE, StringComparison.InvariantCultureIgnoreCase)
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
