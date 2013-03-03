using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
	public class PageFile : Item
    {
        const string XRC_INDEX_FILE = "index";
        const string XRC_LAYOUT_FILE = "_layout";
        readonly static Regex _xrcFileRegEx = new Regex(@"^(?<name>.+)(?<ext>(\.xrc|\.xrc\.\w+))$", RegexOptions.Compiled);

        public PageFile(string resourceName)
            : base(ItemType.PageFile, resourceName, GetFileLogicalName(resourceName))
        {
        }

        string ParentResourceLocation
        {
            get
            {
                if (Parent == null)
                    return "/";
                else
                    return Parent.ResourceLocation;
            }
        }

        public override string ResourceLocation
        {
            get
            {
                return UriExtensions.Combine(ParentResourceLocation, ResourceName);
            }
        }

        static string GetFileLogicalName(string fileName)
        {
            var match = _xrcFileRegEx.Match(fileName.ToLowerInvariant());
            if (!match.Success)
                throw new NotSupportedException(string.Format("Not valid filename '{0}'.", fileName));

            return match.Groups["name"].Value;
        }

        public bool IsIndex
        {
            get
            {
                return string.Equals(Name, XRC_INDEX_FILE, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public bool IsLayout
        {
            get
            {
                return string.Equals(Name, XRC_LAYOUT_FILE, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public bool IsSlot
        {
            get
            {
                return Name.StartsWith("_");
            }
        }

        public PageFile DefaultLayoutFile
        {
            get
            {
                if (Parent == null)
                    return null;

                return Parent.DefaultLayoutFile;
            }
        }
    }
}
