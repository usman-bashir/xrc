using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc.Script
{
    public class ScriptExpression : IScriptExpression
    {
        private Delegate _compiledExpression;
        private object _lock = new object();
        private string _value;
        private Modules.ModuleDefinitionList _modules;

        public ScriptExpression(string textExpression, Delegate compiledDelegate, Modules.ModuleDefinitionList modules)
        {
			_value = textExpression;
			_compiledExpression = compiledDelegate;
            _modules = modules;
        }

        public Modules.ModuleDefinitionList Modules 
        {
            get { return _modules; }
        }

        public override string ToString()
        {
            return _value;
        }

		public Delegate CompiledExpression
		{
            get
            {
                return _compiledExpression;
            }
		}

        public Type ReturnType
        {
            get { return _compiledExpression.Method.ReturnType; }
        }
    }
}
