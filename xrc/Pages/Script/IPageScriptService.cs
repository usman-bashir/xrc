using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.Pages.Script
{
    public interface IPageScriptService
    {
        XValue Parse(string expression, Type returnType, Modules.ModuleDefinitionList modules, PageParameterList parameters);

        object Eval(XValue value, Dictionary<string, Modules.IModule> modules, ContextParameterList parameters);

        bool TryExtractScript(string text, out string script);
    }
}
