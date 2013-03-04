using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace xrc.Views
{
   
    [TestClass()]
    public class ViewCatalogService_Test
    {
        [TestMethod()]
        public void It_should_be_possible_to_get_all_views()
        {
			var container = new WindsorContainer();

            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Core"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.Markdown"));
            XrcWindsor.InstallExtension(container, System.Reflection.Assembly.Load("xrc.FileSystemPages"));

			var target = new WindsorViewCatalogService(container.Kernel);

            Assert.AreEqual(10, target.GetAll().Count());
			Assert.AreEqual(typeof(HtmlView).Name, target.Get(typeof(HtmlView).Name).Name);
            Assert.AreEqual(typeof(MarkdownView), target.Get(typeof(MarkdownView).Name).Type);
		}

		[TestMethod()]
		public void It_should_be_possible_to_get_a_custom_view()
		{
			var container = new WindsorContainer();

			container.Register(Component.For<TestView>()
							.LifeStyle.Transient);

			var target = new WindsorViewCatalogService(container.Kernel);

			Assert.AreEqual(1, target.GetAll().Count());
			Assert.AreEqual(typeof(TestView), target.Get(typeof(TestView).Name).Type);
		}

		[TestMethod()]
		public void It_should_be_possible_to_get_a_view_with_a_specified_name()
		{
			var container = new WindsorContainer();

			container.Register(Component.For<TestView>()
							.Named("CustomName")
							.LifeStyle.Transient);

			var target = new WindsorViewCatalogService(container.Kernel);

			Assert.AreEqual(1, target.GetAll().Count());
			Assert.AreEqual(typeof(TestView), target.Get("CustomName").Type);
		}

    }
}
