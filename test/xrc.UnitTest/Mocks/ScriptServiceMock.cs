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

        public IScriptExpression Parse(string expression, ScriptParameterList parameters)
        {
            return new ScriptExpressionMock(expression, _evalObject != null ? _evalObject.GetType() : typeof(object));
        }

        public object Eval(IScriptExpression expression, ScriptParameterList parameters)
        {
            return _evalObject;
        }
    }
}
