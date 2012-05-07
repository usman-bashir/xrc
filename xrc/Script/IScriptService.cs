using System;
using System.Collections.Generic;

namespace xrc.Script
{
    public interface IScriptService
    {
		IScriptExpression Parse(string script, Modules.ModuleDefinitionList modules, Type returnType);

		bool TryExtractInlineScript(string text, out string script);

        object Eval(IScriptExpression expression, IContext context);
	}
}
