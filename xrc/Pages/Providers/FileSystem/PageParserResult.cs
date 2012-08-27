using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using xrc.Sites;

namespace xrc.Pages.Providers.FileSystem
{
    public class PageParserResult
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
	}
}
