using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
	public class PageDirectory : Item
    {
        const string XRC_SHARED_DIRECTORY = "shared";
        const string XRC_DIRECTORY_CONFIG_FILE = "xrc.config";

        readonly PageDirectoryList _directories;
        readonly PageFileList _files;
        ConfigFile _configFile;

        protected PageDirectory(string resourceName, string name, params Item[] items)
            :base(ItemType.Directory, resourceName, name)
        {
            _directories = new PageDirectoryList(this);
            _files = new PageFileList(this);

            if (items != null)
            {
                _files.AddRange(items.Where(p => p is PageFile).Select(p => (PageFile)p));
                _directories.AddRange(items.Where(p => p is PageDirectory).Select(p => (PageDirectory)p));
                ConfigFile = items.FirstOrDefault(p => p is ConfigFile) as ConfigFile;
            }
        }

        public PageDirectory(string resourceName, params Item[] items)
            : this(resourceName, GetDirectoryLogicalName(resourceName), items)
        {
        }

        public PageFileList Files
        {
            get { return _files; }
        }

        public PageDirectoryList Directories
        {
            get { return _directories; }
        }

        static string GetDirectoryLogicalName(string resourceName)
		{
            return resourceName.ToLowerInvariant();
		}

        public override string ResourceLocation
        {
            get
            {
                if (IsRoot)
                    return UriExtensions.AppendTrailingSlash(ResourceName);
                else
                    return UriExtensions.AppendTrailingSlash(UriExtensions.Combine(Parent.ResourceLocation, ResourceName));
            }
        }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public PageFile IndexFile
        {
            get
            {
                return Files.FirstOrDefault(p => p.ItemType == TreeStructure.ItemType.PageFile
                                                && ((PageFile)p).IsIndex)
                                                as PageFile;
            }
        }

        public PageFile DefaultLayoutFile
        {
            get
            {
                if (LocalLayoutFile != null)
                    return LocalLayoutFile;
                else if (SharedFolder != null && SharedFolder.LocalLayoutFile != null)
                    return SharedFolder.LocalLayoutFile;
                else if (Parent != null)
                    return Parent.DefaultLayoutFile;
                else
                    return null;
            }
        }

        PageFile LocalLayoutFile
        {
            get 
            {
                return Files.FirstOrDefault(p => p.ItemType == TreeStructure.ItemType.PageFile
                                                && ((PageFile)p).IsDefaultLayout)
                                                as PageFile;
            }
        }

        PageDirectory SharedFolder
        {
            get
            {
                return Directories.FirstOrDefault(p => p.ItemType == TreeStructure.ItemType.Directory
                                                && string.Equals(p.Name, XRC_SHARED_DIRECTORY, StringComparison.InvariantCultureIgnoreCase))
                                                as PageDirectory;
            }
        }

        public ConfigFile ConfigFile
        {
            get
            {
                return _configFile;
            }
            set
            {
                _configFile = value;
                if (_configFile != null)
                    _configFile.Parent = this;
            }
        }
    }
}
