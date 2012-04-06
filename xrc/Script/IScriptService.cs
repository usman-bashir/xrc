using System;
using System.Collections.Generic;

namespace xrc.Script
{
    public interface IScriptService
    {
		IScriptExpression Parse(string script, Dictionary<string, Type> arguments, Type returnType);

		bool TryExtractInlineScript(string text, out string expression);
	}
}
