using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.Mocks
{
    class ScriptExpressionMock : IScriptExpression
    {
        string _script;
        Type _returnType;
        public ScriptExpressionMock(string script, Type returnType)
        {
            _script = script;
            _returnType = returnType;
        }

        public override string ToString()
        {
            return _script;
        }

        Modules.ModuleDefinitionList _modules = new Modules.ModuleDefinitionList();
        public Modules.ModuleDefinitionList Modules
        {
            get { return _modules; }
        }

        public Type ReturnType
        {
            get { return _returnType; }
        }
    }
}
