using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xrc.Pages.TreeStructure
{
	public abstract class Item
    {
        readonly ItemType _itemType;
        readonly string _resourceName;
        readonly string _name;
        readonly ParametricUriSegment _parametricUriSegment;
        PageDirectory _parent;

		protected Item(ItemType itemType, string resourceName, string name)
		{
			_resourceName = resourceName;
			_itemType = itemType;
			_name = name;
            _parametricUriSegment = new ParametricUriSegment(_name);
		}

		public string ResourceName
		{
			get { return _resourceName; }
		}

        public abstract string ResourceLocation
        {
            get;
        }

		public string Name
		{
			get { return _name; }
		}

		public PageDirectory Parent
		{
			get { return _parent;}
			internal set { _parent = value;}
		}

		public ItemType ItemType
		{
			get { return _itemType; }
		}

		public override string ToString()
		{
			return ResourceLocation;
		}

        public int Priority
        {
            get
            {
                return 100 - _parametricUriSegment.FixedCharacters;
            }
        }

        public ParametricUriSegmentResult Match(string url)
        {
            return _parametricUriSegment.Match(url);
        }

        public XrcUrl BuildUrl(UriSegmentParameterList segmentParameters = null)
        {
            string currentName = BuildSegmentUrl(segmentParameters);

            XrcUrl url;
            if (ItemType == ItemType.Directory)
            {
                if (Parent == null)
                    url = new XrcUrl(currentName);
                else
                    url = Parent.BuildUrl(segmentParameters).Append(currentName);

                url = url.AppendTrailingSlash();
            }
            else if (ItemType == ItemType.PageFile)
            {
                XrcUrl parentUrl;
                if (Parent == null)
                    parentUrl = new XrcUrl("~");
                else
                    parentUrl = Parent.BuildUrl(segmentParameters);

                if (((PageFile)this).IsIndex)
                    url = parentUrl;
                else
                    url = parentUrl.Append(currentName);
            }
            else
                throw new XrcException(string.Format("BuildUrl not supported on '{0}'.", ItemType));

            return url;
        }

         string BuildSegmentUrl(UriSegmentParameterList segmentParameters = null)
         {
             if (_parametricUriSegment != null)
             {
                 string paramValue;
                 if (segmentParameters == null || !_parametricUriSegment.IsParametric)
                     paramValue = Name;
                 else if (!segmentParameters.TryGetValue(_parametricUriSegment.ParameterName, out paramValue))
                     paramValue = Name;

                 return _parametricUriSegment.BuildSegmentUrl(paramValue);
             }
             else
                 return Name;
         }

	}
}
