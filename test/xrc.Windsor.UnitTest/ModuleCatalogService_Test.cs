using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace xrc.Modules
{
   
    [TestClass()]
    public class ModuleCatalogService_Test
    {
        [TestMethod()]
        public void It_should_be_possible_to_get_all_service_and_module()
        {
			var container = new WindsorContainer();

            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Core"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Markdown"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.FileSystemPages"));

			var target = new WindsorModuleCatalogService(container.Kernel);

            Assert.AreEqual(14, target.GetAll().Count());
			Assert.AreEqual(typeof(UrlModule).Name, target.Get(typeof(UrlModule).Name).Name);
			Assert.AreEqual(typeof(IUrlModule), target.Get(typeof(UrlModule).Name).Type);
			Assert.AreEqual(typeof(XrcService).Name, target.Get(typeof(XrcService).Name).Name);
			Assert.AreEqual(typeof(IXrcService), target.Get(typeof(XrcService).Name).Type);

            Assert.AreEqual(typeof(xrc.Markdown.IMarkdownService), target.Get("MarkdownService").Type);
            Assert.AreEqual(typeof(xrc.Pages.Providers.IPageProviderService), target.Get("PageProviderService").Type);
        }

		[TestMethod()]
		public void It_should_be_possible_to_get_a_custom_module_and_service()
		{
			var container = new WindsorContainer();

			container.Register(Component.For<TestModule>()
							.LifeStyle.Transient);

			container.Register(Component.For<TestService>()
							.LifeStyle.Singleton);

			var target = new WindsorModuleCatalogService(container.Kernel);

			Assert.AreEqual(2, target.GetAll().Count());
			Assert.AreEqual(typeof(TestModule).Name, target.Get(typeof(TestModule).Name).Name);
			Assert.AreEqual(typeof(TestModule), target.Get(typeof(TestModule).Name).Type);
			Assert.AreEqual(typeof(TestService).Name, target.Get(typeof(TestService).Name).Name);
			Assert.AreEqual(typeof(TestService), target.Get(typeof(TestService).Name).Type);
		}

		[TestMethod()]
		public void It_should_be_possible_to_get_a_custom_module_with_a_custom_name()
		{
			var container = new WindsorContainer();

			container.Register(Component.For<TestModule>()
							.Named("CustomNameModule")
							.LifeStyle.Transient);

			var target = new WindsorModuleCatalogService(container.Kernel);

			Assert.AreEqual(1, target.GetAll().Count());
			Assert.AreEqual(typeof(TestModule), target.Get("CustomNameModule").Type);
		}
	}
}
