using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;

namespace xrc
{
    public abstract class ComponentCatalogService<T>
    {
        private ComponentDefinitionList<T> _components = new ComponentDefinitionList<T>();

        public ComponentCatalogService(ComponentCollection components)
        {
            foreach (ComponentElement c in components)
            {
                if (c.Type != null)
                    _components.Add(new ComponentDefinition(c.Name, c.Type));
                else
                {
                    Assembly assembly = Assembly.Load(c.Assembly);
                    LoadFromAssembly(assembly, c.TypePattern);
                }
            }
        }

        private void LoadFromAssembly(Assembly assembly, string typeFullNamePattern)
        {
			var regEx = new Regex(typeFullNamePattern);

            var types = from t in assembly.GetTypes()
                        where typeof(T).IsAssignableFrom(t)
                            && t.IsClass
							&& !t.IsAbstract
							&& regEx.IsMatch(t.FullName)
                        select t;

            foreach (var t in types)
                _components.Add(new ComponentDefinition(t.Name, t));
        }

        public ComponentDefinition Get(string name)
        {
            return _components[name];
        }

		public bool TryGet(string name, out ComponentDefinition component)
		{
			return _components.TryGet(name, out component);
		}

        public IEnumerable<ComponentDefinition> GetAll()
        {
            return _components;
        }
    }
}
