using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using System.Web;

namespace xrc.Windsor.Installers
{
    public class XrcInstaller : IWindsorInstaller
    {
		Configuration.XrcSection _xrcSection;

		public XrcInstaller(Configuration.XrcSection xrcSection)
		{
			_xrcSection = xrcSection;
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            Assembly coreAssembly = typeof(xrc.IKernel).Assembly;

			//container.AddFacility<TraceFacility>();

			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			container.Register(Component.For<xrc.Configuration.IFileSystemConfig>().Instance(new Configuration.FileSystemConfig(_xrcSection)));
			container.Register(Component.For<xrc.Configuration.ICustomErrorsConfig>().Instance(new Configuration.CustomErrorsConfig(_xrcSection)));

			container.Register(Component.For<xrc.Configuration.IEnvironmentConfig>().ImplementedBy<Configuration.EnvironmentConfig>());
			container.Register(Component.For<xrc.Configuration.IHostingConfig>().ImplementedBy<Configuration.AspNetHostingConfig>());

			container.Register(Component.For<Views.IViewCatalogService>().ImplementedBy<Views.WindsorViewCatalogService>());
			container.Register(Component.For<Modules.IModuleCatalogService>().ImplementedBy<Modules.WindsorModuleCatalogService>());
			container.Register(Component.For<Views.IViewFactory>().ImplementedBy<Views.WindsorViewFactory>());
			container.Register(Component.For<Modules.IModuleFactory>().ImplementedBy<Modules.WindsorModuleFactory>());

            container.Register(Component.For<IKernel>().ImplementedBy<Kernel>());

            container.Register(Classes.FromAssembly(coreAssembly)
                                .Where(p => p.Name.EndsWith("Service"))
                                .WithServiceAllInterfaces()
                                .LifestyleSingleton());

            container.Register(Classes.FromAssembly(coreAssembly)
                                .BasedOn<xrc.Views.IView>()
								.WithServiceSelf()
                                .LifestyleTransient());

            container.Register(Classes.FromAssembly(coreAssembly)
                                .Where(p => p.Name.EndsWith("Module") &&
											!typeof(IHttpModule).IsAssignableFrom(p))
								.WithServiceAllInterfaces()
                                .LifestyleTransient());

			container.Register(Classes.FromAssembly(coreAssembly)
								.BasedOn<IHttpModule>()
								.WithServiceAllInterfaces()
								.LifestyleTransient());
		}
    }
}
