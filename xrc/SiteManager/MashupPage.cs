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
        private MashupParameterList _parameters = new MashupParameterList();
        private ModuleDefinitionList _modules = new ModuleDefinitionList();

        public MashupPage()
        {
        }

        public MashupActionList Actions
        {
            get { return _actions; }
        }

        public MashupParameterList Parameters
        {
            get { return _parameters; }
        }

        public ModuleDefinitionList Modules
        {
            get { return _modules; }
        }
    }
}
