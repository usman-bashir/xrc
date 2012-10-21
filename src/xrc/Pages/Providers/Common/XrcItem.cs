﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.Providers.Common
{
	// TODO Valutare se fare più classi

	public sealed class XrcItem
    {
		XrcItem _parent;
		readonly XrcItemType _itemType;
		readonly string _resourceName;

		readonly string _name;
		readonly XrcItemList _items;
		readonly ParametricUriSegment _parametricSegment;

		private XrcItem(XrcItemType itemType, string resourceName,
						string name, ParametricUriSegment parametricSegment,
						XrcItem[] items)
		{
			_resourceName = resourceName;
			_itemType = itemType;
			_name = name;
			_parametricSegment = parametricSegment;

			_items = new XrcItemList(this);
			if (items != null)
				Items.AddRange(items);
		}

		public static XrcItem NewRoot(params XrcItem[] items)
		{
			return new XrcItem(XrcItemType.Directory, "~", "~", null, items);
		}
		public static XrcItem NewDirectory(string resourceName, params XrcItem[] items)
		{
			string name = GetDirectoryLogicalName(resourceName);
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(XrcItemType.Directory, resourceName, name, parametricSegment, items);
		}
		public static XrcItem NewXrcFile(string resourceName)
		{
			string name = GetFileLogicalName(resourceName);
			var parametricSegment = new ParametricUriSegment(name);

			return new XrcItem(XrcItemType.XrcFile, resourceName, name, parametricSegment, null);
		}
		public static XrcItem NewConfigFile(string resourceName)
		{
			string name = GetConfigLogicalName(resourceName);

			return new XrcItem(XrcItemType.ConfigFile, resourceName, name, null, null);
		}

		public string ResourceName
		{
			get { return _resourceName; }
		}

		public string Name
		{
			get { return _name; }
		}

		public string ResourceLocation
		{
			get
			{
				if (ItemType == XrcItemType.Directory)
				{
					if (IsRoot)
						return UriExtensions.AppendTrailingSlash(ResourceName);
					else
						return UriExtensions.AppendTrailingSlash(UriExtensions.Combine(Parent.ResourceLocation, ResourceName));
				}
				else
					return UriExtensions.Combine(Parent.ResourceLocation, ResourceName);
			}
		}

		public XrcUrl GetUrl(Dictionary<string, string> segmentParameters = null)
		{
			string currentName;
			if (segmentParameters != null && 
				_parametricSegment != null && _parametricSegment.IsParametric)
			{
				string paramValue;
				if (segmentParameters.TryGetValue(_parametricSegment.ParameterName, out paramValue))
					currentName = paramValue;
				else
					currentName = Name;
			}
			else
				currentName = Name;

			XrcUrl url;
			if (ItemType == XrcItemType.Directory)
			{
				if (IsRoot)
					url = new XrcUrl(currentName);
				else
					url = Parent.GetUrl(segmentParameters).Append(currentName);

				url = url.AppendTrailingSlash();
			}
			else
				url = Parent.GetUrl(segmentParameters).Append(currentName);

			return url;
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
		private static Regex _xrcFileRegEx = new Regex(@"^(?<name>.+)(?<ext>(\.xrc|\.xrc\.\w+))$", RegexOptions.Compiled);

		public static string GetFileLogicalName(string fileName)
		{
			var match = _xrcFileRegEx.Match(fileName.ToLowerInvariant());
			if (!match.Success)
				throw new NotSupportedException(string.Format("Not valid filename '{0}'.", fileName));

			return match.Groups["name"].Value;
		}

		public static string GetDirectoryLogicalName(string directoryName)
		{
			return directoryName.ToLowerInvariant();
		}

		public static string GetConfigLogicalName(string configName)
		{
			if (!string.Equals(XRC_DIRECTORY_CONFIG_FILE, configName, StringComparison.InvariantCultureIgnoreCase))
				throw new NotSupportedException(string.Format("Not valid filename '{0}'.", configName));

			return configName.ToLowerInvariant();
		}
		//public static string GetFileExtension(string fileName)
		//{
		//    var match = _xrcFileRegEx.Match(fileName.ToLowerInvariant());
		//    if (!match.Success)
		//        throw new NotSupportedException(string.Format("Not valid filename '{0}'.", fileName));

		//    return match.Groups["ext"].Value.ToLowerInvariant();
		//}
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
