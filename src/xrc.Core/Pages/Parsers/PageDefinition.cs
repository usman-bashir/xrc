﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Pages.Parsers
{
    public class PageDefinition
    {
		PageActionList _actions = new PageActionList();
		PageParameterList _parameters = new PageParameterList();
		ModuleDefinitionList _modules = new ModuleDefinitionList();

		public PageActionList Actions
		{
			get { return _actions; }
		}

		public PageParameterList Parameters
		{
			get { return _parameters; }
		}

		public ModuleDefinitionList Modules
		{
			get { return _modules; }
		}

		public PageDefinition Combine(PageDefinition other)
		{
			var result = new PageDefinition();

			foreach (var a in Actions)
				result.Actions[a.Method] = a;
			foreach (var a in other.Actions)
				result.Actions[a.Method] = a;

			foreach (var a in Parameters)
				result.Parameters[a.Name] = a;
			foreach (var a in other.Parameters)
				result.Parameters[a.Name] = a;

			foreach (var a in Modules)
				result.Modules[a.Name] = a;
			foreach (var a in other.Modules)
				result.Modules[a.Name] = a;

			return result;
		}
	}
}
