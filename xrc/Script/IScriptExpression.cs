using System;
namespace xrc.Script
{
    public interface IScriptExpression
    {
		object Eval(params object[] args);

		TResults Eval<TP1, TResults>(TP1 arg1);
		TResults Eval<TP1, TP2, TResults>(TP1 arg1, TP2 arg2);
		TResults Eval<TP1, TP2, TP3, TResults>(TP1 arg1, TP2 arg2, TP3 arg3);
	}
}
