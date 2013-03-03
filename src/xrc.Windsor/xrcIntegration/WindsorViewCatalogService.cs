﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public class WindsorViewCatalogService : IViewCatalogService
    {
        readonly WindsorContainer _container;
		readonly Lazy<IEnumerable<ComponentDefinition>> _components;

        public WindsorViewCatalogService(WindsorContainer container)
        {
            _container = container;

			_components = new Lazy<IEnumerable<ComponentDefinition>>(LoadComponents);
        }

		IEnumerable<ComponentDefinition> LoadComponents()
		{
            var handlers = _container.Kernel.GetAssignableHandlers(typeof(IView));

			var components = from h in handlers
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
				throw new XrcException(string.Format("Cannot find View '{0}' in '{1}'.", name, this.GetType().Name));

			return c;
		}

		public bool TryGet(string name, out ComponentDefinition component)
		{
			component = _components.Value.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if (component == null)
				return false;

			return true;
		}

        public void RegisterAll()
        {
            var assemblyFilter = new AssemblyFilter(AssemblyDirectory);

            _container.Register(Classes.FromAssemblyInDirectory(assemblyFilter)
                                .BasedOn<IView>()
                                .WithServiceAllInterfaces()
                                .LifestyleTransient());
        }

        string AssemblyDirectory
        {
            get
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

                var uri = new UriBuilder(codeBase);

                var path = Uri.UnescapeDataString(uri.Path);

                return System.IO.Path.GetDirectoryName(path);
            }
        }
    }
}
