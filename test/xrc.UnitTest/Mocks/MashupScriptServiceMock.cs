using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;
using System.Linq.Expressions;
using xrc.Pages.Script;
using xrc.Pages;

namespace xrc.Mocks
{
    class PageScriptServiceMock : IPageScriptService
    {
		public PageScriptServiceMock()
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

        public XValue Parse(string expression, Type returnType, Modules.ModuleDefinitionList modules, PageParameterList parameters)
        {
            string script;
            if (TryExtractScript(expression, out script))
                return new XValue(returnType, new ScriptExpressionMock(script, returnType));
            else
                return new XValue(returnType, Convert.ChangeType(expression, returnType, System.Globalization.CultureInfo.InvariantCulture));
        }

        public object Eval(XValue value, Dictionary<string, object> modules, ContextParameterList parameters)
        {
            return value.Expression.ToString();
        }
    }
}
