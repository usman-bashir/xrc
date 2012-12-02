using Castle.MicroKernel.Registration;

namespace xrc.Web.AcceptanceTest.Installers
{
    public class CustomXrcInstaller : IWindsorInstaller
    {
		public CustomXrcInstaller()
		{
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
			container.Register(Classes.FromAssemblyContaining<TestView>()
					.BasedOn<xrc.Views.IView>()
					.WithServiceSelf()
					.LifestyleTransient());

			container.Register(Classes.FromAssemblyContaining<ContactModule>()
								.Where(p => p.Name.EndsWith("Module"))
								.WithServiceSelf()
								.WithServiceDefaultInterfaces()
								.LifestyleTransient());
		}
    }
}
