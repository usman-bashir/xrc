using Castle.MicroKernel.Registration;

namespace $rootnamespace$.Installers
{
    public class CustomXrcInstaller : IWindsorInstaller
    {
		public CustomXrcInstaller()
		{
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.MVC4"));
            xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.FileSystemPages"));

			// TODO Add your components here
		}
    }
}
