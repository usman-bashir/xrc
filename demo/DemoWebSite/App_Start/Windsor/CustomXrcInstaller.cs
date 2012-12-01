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
			// Register demo web site modules
			container.Register(Classes.FromAssemblyContaining<TwitterModule>()
								.Where(p => p.Name.EndsWith("Module"))
								.WithServiceSelf()
								.WithServiceDefaultInterfaces()
								.LifestyleTransient());
			// Register demo web site views
			container.Register(Classes.FromAssemblyContaining<DemoWebSiteView>()
					.BasedOn<xrc.Views.IView>()
					.WithServiceSelf() // currently views doesn't have an interface 
					.LifestyleTransient());
		}
    }
}
