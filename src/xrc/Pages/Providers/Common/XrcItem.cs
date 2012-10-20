using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common
{
	// TODO Valutare se fare più classi

	public sealed class XrcItem
    {
		XrcItem _parent;
		readonly XrcItemType _itemType;
		readonly string _id;
		readonly string _fileName;

		readonly string _name;
		readonly string _fileExtension;
		readonly XrcItemList _items;
		readonly ParametricUriSegment _parametricSegment;

		private XrcItem(XrcItemType itemType, string id, string fileName,
						string name, ParametricUriSegment parametricSegment,
						string fileExtension, XrcItem[] items)
		{
			_id = id;
			_fileName = fileName;
			_itemType = itemType;
			_name = name;
			_parametricSegment = parametricSegment;
			_fileExtension = fileExtension;

			_items = new XrcItemList(this);
			if (items != null)
				Items.AddRange(items);
		}

		public static XrcItem NewRoot(string id, params XrcItem[] items)
		{
			return new XrcItem(XrcItemType.Directory, id, null, "~", null, null, items);
		}
		public static XrcItem NewDirectory(string id, string directoryName, params XrcItem[] items)
		{
			string name = GetDirectoryLogicalName(directoryName);
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(XrcItemType.Directory, id, directoryName, name, parametricSegment, null, items);
		}
		public static XrcItem NewXrcFile(string id, string fileName)
		{
			string name = GetFileLogicalName(fileName);
			string fileExtension = GetFileExtension(fileName);
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(XrcItemType.XrcFile, id, fileName, name, parametricSegment, fileExtension, null);
		}
		public static XrcItem NewConfigFile(string id, string fileName)
		{
			string name = GetConfigLogicalName(fileName);
			string fileExtension = GetFileExtension(fileName);

			return new XrcItem(XrcItemType.ConfigFile, id, fileName, name, null, fileExtension, null);
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
			get 
			{
				if (ItemType == XrcItemType.Directory)
				{
					if (IsRoot)
						return UriExtensions.AppendTrailingSlash(Name);
					else
						return UriExtensions.AppendTrailingSlash(UriExtensions.Combine(Parent.VirtualPath, Name));
				}
				else if (Parent != null)
					return UriExtensions.Combine(Parent.VirtualPath, Name);
				else
					return UriExtensions.Combine("~/", Name);
			}
		}

		public string Id
		{
			get { return _id; }
		}

		public XrcItem Parent
		{
			get { return _parent;}
			internal set { _parent = value;}
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

		public XrcItemList Items
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

	public class XrcItemList : IEnumerable<XrcItem>
	{
		readonly List<XrcItem> _list = new List<XrcItem>();
		readonly XrcItem _parent;

		public XrcItemList(XrcItem parent)
		{
			_parent = parent;
		}

		public void Add(XrcItem item)
		{
			item.Parent = _parent;
			_list.Add(item);
		}

		public void AddRange(IEnumerable<XrcItem> items)
		{
			foreach (var i in items)
				Add(i);
		}

		public IEnumerator<XrcItem> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}

}
