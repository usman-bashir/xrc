using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.Reflection;

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
                    LoadFromAssembly(assembly);
                }
            }
        }

        private void LoadFromAssembly(Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where typeof(T).IsAssignableFrom(t)
                        select t;

            foreach (var t in types)
                _components.Add(new ComponentDefinition(t.Name, t));
        }

        public ComponentDefinition Get(string name)
        {
            return _components[name];
        }

        public IEnumerable<ComponentDefinition> GetAll()
        {
            return _components;
        }
    }
}
