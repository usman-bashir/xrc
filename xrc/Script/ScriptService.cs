using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using xrc.Modules;

namespace xrc.Script
{
    public class ScriptService : IScriptService
    {
        //_xmlExpRegEx = new Regex(@"^\s*@\{(?<code>.+)\}\s*$", RegexOptions.Compiled);
        private static Regex _xmlExpRegEx = new Regex(@"^\s*@(?<code>.+)\s*$", RegexOptions.Compiled);

        private IModuleFactory _moduleFactory;

        public ScriptService(IModuleFactory moduleFactory)
		{
            _moduleFactory = moduleFactory;
		}

        public IScriptExpression Parse(string script, Modules.ModuleDefinitionList modules, Type returnType)
        {
            var argParameters = modules.Select(p => Expression.Parameter(p.Component.Type, p.Name))
                                .Concat(new ParameterExpression[]{Expression.Parameter(typeof(IContext), "Context")})
                                .ToArray();
			var function = DynamicExpression.FunctionFactory.Create(returnType, script, argParameters);

			return new ScriptExpression(script, function, modules);
        }

		public bool TryExtractInlineScript(string text, out string expression)
		{
			expression = null;

			var match = _xmlExpRegEx.Match(text);
			if (!match.Success)
				return false;

			expression = match.Groups["code"].Value;
			if (expression != null)
				expression = expression.Trim();

			return true;
		}


        public object Eval(IScriptExpression expression, IContext context)
        {
            ScriptExpression scriptExpression = (ScriptExpression)expression;

            var modules = expression.Modules.Select(p => _moduleFactory.Get(p.Component, context));
            try
            {
                object[] arguments = modules.Concat(new object[] { context }).ToArray();
                return scriptExpression.CompiledExpression.DynamicInvoke(arguments);
            }
            finally
            {
                foreach (var m in modules)
                    _moduleFactory.Release(m);
            }
        }
    }
}
