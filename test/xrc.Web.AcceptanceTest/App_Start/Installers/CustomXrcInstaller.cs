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
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.GetExecutingAssembly());
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.MVC4"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Markdown"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.FileSystemPages"));
        }
    }
}
