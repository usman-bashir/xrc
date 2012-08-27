using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.Pages
{
    public class PageParameter
    {
        public PageParameter(string name, XValue xvalue, bool allowRequestOverride = false)
        {
            _name = name;
            _xValue = xvalue;
            _allowRequestOverride = allowRequestOverride;
        }

        private XValue _xValue;
        public XValue Value
        {
            get { return _xValue; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private bool _allowRequestOverride;
        public bool AllowRequestOverride
        {
            get { return _allowRequestOverride; }
        }
    }
}
