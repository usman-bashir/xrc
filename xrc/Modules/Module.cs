using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
	public class Module
	{
		public Module(string name, Type type)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
			if (type == null)
				throw new ArgumentNullException("type");

			Name = name;
			ModuleType = type;
		}

		public string Name
		{
			get;
			private set;
		}

		public Type ModuleType
		{
			get;
			private set;
		}
	}
}
