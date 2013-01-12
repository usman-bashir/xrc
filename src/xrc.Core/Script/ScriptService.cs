using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using xrc.Modules;
using System.Globalization;

namespace xrc.Script
{
    public class ScriptService : IScriptService
    {
        readonly DynamicExpresso.ExpressionEngine _engine;
        public ScriptService()
		{
            _engine = new DynamicExpresso.ExpressionEngine();
		}

        public IScriptExpression Parse(string script, ScriptParameterList parameters)
        {
            var exp = _engine.Parse(script, parameters.Select(p => new DynamicExpresso.ExpressionParameter(p.Name, p.Type, p.Value)).ToArray());

            return new ScriptExpression(exp);
        }

        public object Eval(IScriptExpression expression, ScriptParameterList parameters)
        {
            ScriptExpression scriptExpression = (ScriptExpression)expression;

            var args = parameters.Select(p => new DynamicExpresso.ExpressionParameter(p.Name, p.Type, p.Value)).ToArray();

            return scriptExpression.ExpressionDefinition.Eval(args);
        }
    }
}
