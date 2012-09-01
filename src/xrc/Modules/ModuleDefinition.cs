using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
	public class ModuleDefinition
	{
        public ModuleDefinition(string name, ComponentDefinition component)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
            if (component == null)
                throw new ArgumentNullException("component");

			Name = name;
            Component = component;
		}

		public string Name
		{
			get;
			private set;
		}

        public ComponentDefinition Component
		{
			get;
			private set;
		}
	}
}
