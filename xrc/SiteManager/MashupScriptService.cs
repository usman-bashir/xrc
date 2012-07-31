using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Configuration;
using System.Reflection;
using xrc.Script;
using System.Globalization;
using xrc.Renderers;
using xrc.Modules;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace xrc.SiteManager
{
    public class MashupScriptService : IMashupScriptService
    {
        //_xmlExpRegEx = new Regex(@"^\s*@\{(?<code>.+)\}\s*$", RegexOptions.Compiled);
        private static Regex _xmlExpRegEx = new Regex(@"^\s*@(?<code>.+)\s*$", RegexOptions.Compiled);

        private IScriptService _scriptService;
        private IModuleFactory _moduleFactory;

        public MashupScriptService(IScriptService scriptService, IModuleFactory moduleFactory)
        {
            _scriptService = scriptService;
            _moduleFactory = moduleFactory;
        }

        public bool TryExtractScript(string text, out string script)
        {
            script = null;
            if (text == null)
                return false;

            var match = _xmlExpRegEx.Match(text);
            if (!match.Success)
                return false;

            script = match.Groups["code"].Value;
            if (script != null)
                script = script.Trim();

            return true;
        }


        public XValue Parse(string expression, Type returnType, Modules.ModuleDefinitionList modules, MashupParameterList parameters)
        {
            string script;
            if (TryExtractScript(expression, out script))
                return new XValue(returnType, ParseScript(script, returnType, modules, parameters));
            else
            {
                object value = ConvertEx.ChangeType(expression, returnType, CultureInfo.InvariantCulture);
                return new XValue(returnType, value);
            }
        }

        private IScriptExpression ParseScript(string script, Type returnType, Modules.ModuleDefinitionList modules, MashupParameterList parameters)
        {
            var argParameters = new ScriptParameterList();
            foreach (var m in modules)
                argParameters.Add(new ScriptParameter(m.Name, m.Component.Type));
            foreach (var p in parameters)
                argParameters.Add(new ScriptParameter(p.Name, p.Value.ValueType));

            return _scriptService.Parse(script, returnType, argParameters);
        }

        public object Eval(XValue value, Dictionary<string, Modules.IModule> modules, ContextParameterList parameters)
        {
            if (value.Expression != null)
                return EvalScript(value.Expression, modules, parameters);
            else
                return value.Value;
        }

        private object EvalScript(IScriptExpression expression, Dictionary<string, Modules.IModule> modules, ContextParameterList parameters)
        {
            var argParameters = new ScriptParameterList();
            foreach (var m in modules)
                argParameters.Add(new ScriptParameter(m.Key, m.Value.GetType(), m.Value));
            foreach (var p in parameters)
                argParameters.Add(new ScriptParameter(p.Name, p.Type, p.Value));

            return _scriptService.Eval(expression, argParameters);
        }
    }
}
