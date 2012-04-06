using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
	// TODO Da rivedere con IoC (factory?)
	public static class TypeResolverService
	{
		public static Type ResolveType(string typeName)
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();

			return (from t in assembly.GetTypes()
					where t.Name == typeName
					select t).SingleOrDefault();
		}
	}
}
