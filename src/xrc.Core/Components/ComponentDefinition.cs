using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
	public class ComponentDefinition
	{
        public ComponentDefinition(string name, Type type)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
			if (type == null)
				throw new ArgumentNullException("type");

			Name = name;
            Type = type;
		}

		public string Name
		{
			get;
			private set;
		}

		public Type Type
		{
			get;
			private set;
		}
	}
}
