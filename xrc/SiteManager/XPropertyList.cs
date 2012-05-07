using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using xrc.Script;

namespace xrc.SiteManager
{
    public class XPropertyList : IEnumerable<XProperty>
    {
        private Dictionary<string, XProperty> _list = new Dictionary<string, XProperty>(StringComparer.OrdinalIgnoreCase);

        public XPropertyList()
        {
        }

        public void Add(XProperty property)
        {
            _list.Add(property.PropertyInfo.Name, property);
        }

        public XProperty this[string name]
        {
            get
            {
                return _list[name];
            }
        }

        public IEnumerator<XProperty> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item.Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }
    }
}
