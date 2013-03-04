using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using System.Web;
using Castle.Windsor;

namespace xrc
{
    public static class XrcWindsor
    {
        /// <summary>
        /// Register in the windsor container all the core components (views, modules, services, ...).
        /// </summary>
        public static void InstallCore(IWindsorContainer container, Configuration.XrcSection xrcSection)
        {
            Assembly coreAssembly = typeof(xrc.IKernel).Assembly;

			// Enable StackTrace tracing
			//container.AddFacility<Tracing.TraceFacility>();

			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			container.Register(Component.For<xrc.Configuration.IFileSystemConfig>().Instance(new Configuration.FileSystemConfig(xrcSection)));
			container.Register(Component.For<xrc.Configuration.ICustomErrorsConfig>().Instance(new Configuration.CustomErrorsConfig(xrcSection)));

			container.Register(Component.For<xrc.Configuration.IEnvironmentConfig>().ImplementedBy<Configuration.EnvironmentConfig>());
			container.Register(Component.For<xrc.Configuration.IHostingConfig>().ImplementedBy<Configuration.AspNetHostingConfig>());

			container.Register(Component.For<Views.IViewCatalogService>().ImplementedBy<Views.WindsorViewCatalogService>());
			container.Register(Component.For<Modules.IModuleCatalogService>().ImplementedBy<Modules.WindsorModuleCatalogService>());
			container.Register(Component.For<Views.IViewFactory>().ImplementedBy<Views.WindsorViewFactory>());
			container.Register(Component.For<Modules.IModuleFactory>().ImplementedBy<Modules.WindsorModuleFactory>());

            container.Register(Component.For<IKernel>().ImplementedBy<Kernel>());

            InstallExtension(container, coreAssembly);

			container.Register(Classes.FromAssembly(coreAssembly)
								.BasedOn<IHttpModule>()
								.WithServiceAllInterfaces()
								.LifestyleTransient());
		}

        /// <summary>
        /// Register in the windsor container all the components (views, modules, services, parsers, ...) defined in the specified assembly.
        /// Modules and views are registered as transient. Parsers and Services are registered as singleton.
        /// </summary>
        public static void InstallExtension(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Service"))
                                .WithServiceAllInterfaces()
                                .LifestyleSingleton());

            container.Register(Classes.FromAssembly(assembly)
                                .BasedOn<xrc.Pages.Parsers.IResourceParser>()
                                .WithServiceAllInterfaces()
                                .WithServiceSelf()
                                .LifestyleSingleton());

            container.Register(Classes.FromAssembly(assembly)
                                .BasedOn<xrc.Views.IView>()
                                .WithServiceAllInterfaces()
                                .WithServiceSelf()
                                .LifestyleTransient());

            container.Register(Classes.FromAssembly(assembly)
                                .Where(p => p.Name.EndsWith("Module") &&
											!typeof(IHttpModule).IsAssignableFrom(p))
								.WithServiceAllInterfaces()
                                .LifestyleTransient());
		}
    }
}
