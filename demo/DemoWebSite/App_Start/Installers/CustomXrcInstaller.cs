using Castle.MicroKernel.Registration;

namespace DemoWebSite.Windsor
{
    public class CustomXrcInstaller : IWindsorInstaller
    {
		public CustomXrcInstaller()
		{
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
			container.Register(Classes.FromAssemblyContaining<TwitterModule>()
								.Where(p => p.Name.EndsWith("Module"))
								.WithServiceSelf()
								.WithServiceDefaultInterfaces()
								.LifestyleTransient());

			container.Register(Classes.FromAssemblyContaining<DemoWebSiteView>()
					.BasedOn<xrc.Views.IView>()
					.WithServiceSelf()
					.LifestyleTransient());
		}
    }
}
