using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.Mocks
{
    class ScriptServiceMock : IScriptService
    {
        private bool _isScript;
        private object _evalObject;
        private string _script;

        public ScriptServiceMock(bool isScript = false, object evalObject = null, string script = null)
        {
            _script = script;
            _isScript = isScript;
            _evalObject = evalObject;
        }

        public IScriptExpression Parse(string script, Modules.ModuleDefinitionList modules, Type returnType)
        {
            return new ScriptExpressionMock(script, returnType);
        }

        public bool TryExtractInlineScript(string text, out string script)
        {
            script = _script;
            return _isScript;
        }

        public object Eval(IScriptExpression expression, IContext context)
        {
            return _evalObject;
        }
    }
}
