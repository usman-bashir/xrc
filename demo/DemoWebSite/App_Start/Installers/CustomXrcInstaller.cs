using Castle.MicroKernel.Registration;

namespace DemoWebSite.Installers
{
    public class CustomXrcInstaller : IWindsorInstaller
    {
		public CustomXrcInstaller()
		{
		}

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.FileSystemPages"));
            xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("DemoWebSite.Lib"));
            xrc.XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Markdown"));
        }
    }
}
