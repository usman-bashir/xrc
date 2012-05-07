using System;
using System.Collections.Generic;
namespace xrc.Script
{
    public interface IScriptExpression
    {
        Type ReturnType { get; }
        Modules.ModuleDefinitionList Modules { get; }
	}
}
