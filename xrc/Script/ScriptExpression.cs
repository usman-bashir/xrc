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

        public ScriptExpression(string textExpression, Delegate compiledDelegate)
        {
			_value = textExpression;
			_compiledExpression = compiledDelegate;
        }

        public override string ToString()
        {
            return _value;
        }

		public object Eval(params object[] args)
		{
			return _compiledExpression.DynamicInvoke(args);
		}

		public TResults Eval<TP1, TResults>(TP1 arg1)
		{
			return (TResults)Eval(arg1);
		}

		public TResults Eval<TP1, TP2, TResults>(TP1 arg1, TP2 arg2)
		{
			return (TResults)Eval(arg1, arg2);
		}

		public TResults Eval<TP1, TP2, TP3, TResults>(TP1 arg1, TP2 arg2, TP3 arg3)
		{
			return (TResults)Eval(arg1, arg2, arg3);
		}
	}
}
