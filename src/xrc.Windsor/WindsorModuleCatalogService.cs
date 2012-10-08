using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public class WindsorModuleCatalogService : IModuleCatalogService
    {
        readonly Castle.MicroKernel.IKernel _windsorKernel;
		readonly Lazy<IEnumerable<ComponentDefinition>> _components;

		public WindsorModuleCatalogService(Castle.MicroKernel.IKernel windsorKernel)
        {
            _windsorKernel = windsorKernel;

			_components = new Lazy<IEnumerable<ComponentDefinition>>(LoadComponents);
        }

		IEnumerable<ComponentDefinition> LoadComponents()
		{
			var handlers = _windsorKernel.GetAssignableHandlers(typeof(object));

			var validHandlers = from h in handlers
					where h.ComponentModel.Name.EndsWith("Module") ||
							h.ComponentModel.Name.EndsWith("Service")
					select h;

			var components = from h in validHandlers
							 from s in h.ComponentModel.Services
							 select new ComponentDefinition(GetComponentName(h.ComponentModel, s), s);

			return components.ToList();
		}

		string GetComponentName(Castle.Core.ComponentModel component, Type service)
		{
			if (component.ComponentName.SetByUser)
				return component.ComponentName.Name;

			if (service.IsInterface && service.Name.StartsWith("I"))
				return service.Name.TrimStart('I');
			else
				return service.Name;
		}

		public IEnumerable<ComponentDefinition> GetAll()
		{
			return _components.Value;
		}

		public ComponentDefinition Get(string name)
		{
			ComponentDefinition c;
			if (!TryGet(name, out c))
				throw new XrcException(string.Format("Cannot find Module '{0}' in '{1}'.", name, this.GetType().Name));

			return c;
		}

		public bool TryGet(string name, out ComponentDefinition component)
		{
			component = _components.Value.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (component == null)
				return false;

			return true;
		}
	}
}
