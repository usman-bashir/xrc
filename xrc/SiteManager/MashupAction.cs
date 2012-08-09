using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.SiteManager
{
    public class MashupAction
    {
        private ViewDefinitionList _views = new ViewDefinitionList();

        public MashupAction(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("method");
            Method = method.ToLower();
        }

        public string Method
        {
            get;
            private set;
        }

        /// <summary>
        /// Define the parent layout file.
        /// </summary>
        public string Parent
        {
            get;
            set;
        }

        public ViewDefinitionList Views
        {
            get { return _views; }
        }
    }
}
