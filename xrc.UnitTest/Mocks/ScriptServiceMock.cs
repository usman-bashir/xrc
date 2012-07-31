using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;
using System.Linq.Expressions;

namespace xrc.Mocks
{
    class ScriptServiceMock : IScriptService
    {
        private object _evalObject;

        public ScriptServiceMock(object evalObject = null)
        {
            _evalObject = evalObject;
        }

        public IScriptExpression Parse(string expression, Type returnType, ScriptParameterList parameters)
        {
            return new ScriptExpressionMock(expression, returnType);
        }

        public object Eval(IScriptExpression expression, ScriptParameterList parameters)
        {
            return _evalObject;
        }
    }
}
