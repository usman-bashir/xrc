using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;
using System.Linq.Expressions;
using xrc.SiteManager;

namespace xrc.Mocks
{
    class MashupScriptServiceMock : IMashupScriptService
    {
        public MashupScriptServiceMock()
        {
        }

        public bool TryExtractScript(string text, out string script)
        {
            if (string.IsNullOrWhiteSpace(text) == false && text.StartsWith("@"))
            {
                script = text.Substring(1);
                return true;
            }
            else
            {
                script = null;
                return false;
            }
        }

        public XValue Parse(string expression, Type returnType, Modules.ModuleDefinitionList modules, MashupParameterList parameters)
        {
            string script;
            if (TryExtractScript(expression, out script))
                return new XValue(returnType, new ScriptExpressionMock(script, returnType));
            else
                return new XValue(returnType, Convert.ChangeType(expression, returnType, System.Globalization.CultureInfo.InvariantCulture));
        }

        public object Eval(XValue value, Dictionary<string, Modules.IModule> modules, ContextParameterList parameters)
        {
            return value.Expression.ToString();
        }
    }
}
