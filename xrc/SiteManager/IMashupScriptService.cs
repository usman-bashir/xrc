using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Script;

namespace xrc.SiteManager
{
    public interface IMashupScriptService
    {
        XValue Parse(string expression, Type returnType, Modules.ModuleDefinitionList modules, MashupParameterList parameters);

        object Eval(XValue value, Dictionary<string, Modules.IModule> modules, ContextParameterList parameters);

        bool TryExtractScript(string text, out string script);
    }
}
