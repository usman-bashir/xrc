using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.SiteManager
{
    public class MashupPage
    {
        private MashupActionList _actions = new MashupActionList();
        private Dictionary<string, string> _pageParameters = new Dictionary<string, string>();
        private ModuleDefinitionList _modules = new ModuleDefinitionList();

        public MashupPage()
        {
        }

        public MashupActionList Actions
        {
            get { return _actions; }
        }

        public Dictionary<string, string> PageParameters
        {
            get { return _pageParameters; }
        }

        public ModuleDefinitionList Modules
        {
            get { return _modules; }
        }
    }
}
