using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using System.Reflection;

namespace xrc.IoC.Windsor
{
    public class XrcDefaultInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            Assembly assembly = typeof(xrc.IKernel).Assembly;

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
                                .BasedOn<xrc.Renderers.IRenderer>()
                                .WithServiceSelf()
                                .LifestyleTransient());
            container.Register(Classes.FromAssembly(assembly)
                                .BasedOn<xrc.Modules.IModule>()
                                .WithServiceSelf()
                                .LifestyleTransient());
        }
    }
}
