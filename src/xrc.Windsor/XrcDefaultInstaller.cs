using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace xrc.IoC.Windsor
{
    public class XrcDefaultInstaller : IWindsorInstaller
    {
		Configuration.XrcSection _xrcSection;

		public XrcDefaultInstaller(Configuration.XrcSection xrcSection)
		{
			_xrcSection = xrcSection;
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            Assembly assembly = typeof(xrc.IKernel).Assembly;

			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			container.Register(Component.For<xrc.Configuration.IRootPathConfig>().Instance(_xrcSection.RootPath));
			container.Register(Component.For<xrc.Configuration.ISitesConfig>().Instance(_xrcSection).Named("ISitesConfig"));

			container.Register(Component.For<Views.IViewCatalogService>().ImplementedBy<Views.WindsorViewCatalogService>());
			container.Register(Component.For<Modules.IModuleCatalogService>().ImplementedBy<Modules.WindsorModuleCatalogService>());

            container.Register(Component.For<IKernel>()
                                .ImplementedBy<Kernel>()
                                .LifestyleSingleton());

            container.Register(Classes.FromThisAssembly()
                            .Where(p => p.Name.EndsWith("Factory"))
                            .WithServiceAllInterfaces()
                            .LifestyleSingleton());

            container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Service"))
                                .WithServiceAllInterfaces()
                                .LifestyleSingleton());

            container.Register(Classes.FromAssembly(assembly)
                                .BasedOn<xrc.Views.IView>()
								.WithServiceSelf() // currently views doesn't have an interface 
                                .LifestyleTransient());

            container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Module"))
								.WithServiceAllInterfaces()
                                .LifestyleTransient());
        }
    }
}
