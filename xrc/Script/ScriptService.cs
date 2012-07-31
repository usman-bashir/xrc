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
        public ScriptService()
		{
		}

        public IScriptExpression Parse(string script, Type returnType, ScriptParameterList parameters)
        {
            var args = parameters.Select(p => ParameterExpression.Parameter(p.Type, p.Name)).ToArray();

            var function = DynamicExpression.FunctionFactory.Create(returnType, script, args);

            return new ScriptExpression(script, parameters, function);
        }

        public object Eval(IScriptExpression expression, ScriptParameterList parameters)
        {
            ScriptExpression scriptExpression = (ScriptExpression)expression;

            var args = scriptExpression.Parameters.Select(p => parameters[p.Name].Value).ToArray();

            return scriptExpression.CompiledExpression.DynamicInvoke(args);
        }
    }
}
