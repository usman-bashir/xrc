using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace xrc.Script
{
    public class ScriptExpression : IScriptExpression
    {
        private Delegate _compiledExpression;
        private string _value;
        private ScriptParameterList _parameters;

        public ScriptExpression(string textExpression, ScriptParameterList parameters, Delegate compiledDelegate)
        {
			_value = textExpression;
			_compiledExpression = compiledDelegate;
            _parameters = parameters;
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

        public ScriptParameterList Parameters
        {
            get { return _parameters; }
        }
    }
}
