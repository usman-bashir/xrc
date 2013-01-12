using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace xrc.Script
{
    public interface IScriptService
    {
        IScriptExpression Parse(string expression, ScriptParameterList parameters);

        object Eval(IScriptExpression expression, ScriptParameterList parameters);
	}
}
