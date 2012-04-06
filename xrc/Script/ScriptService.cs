using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace xrc.Script
{
    public class ScriptService : IScriptService
    {
		private static Regex _xmlExpRegEx;
		static ScriptService()
        {
            //_xmlExpRegEx = new Regex(@"^\s*@\{(?<code>.+)\}\s*$", RegexOptions.Compiled);
			_xmlExpRegEx = new Regex(@"^\s*@(?<code>.+)\s*$", RegexOptions.Compiled);
		}

		public ScriptService()
		{
		}

		public IScriptExpression Parse(string script, Dictionary<string, Type> arguments, Type returnType)
        {
            var argParameters = arguments.Select(p => Expression.Parameter(p.Value, p.Key)).ToArray();
			var function = DynamicExpression.FunctionFactory.Create(returnType, script, argParameters);

			return new ScriptExpression(script, function);
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
	}
}
