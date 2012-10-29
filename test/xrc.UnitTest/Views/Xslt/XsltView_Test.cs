using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using Moq;
using xrc.Pages.Providers;
using xrc.Pages;

namespace xrc.Views
{
    [TestClass]
    public class XsltView_Test
    {
        [TestMethod]
        public void Transform_Simple_Direct()
        {
			var resourceProvider = new Mock<IResourceProviderService>();
			XsltView target = new XsltView(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null), resourceProvider.Object);
			target.Data = XDocument.Load(TestHelper.GetPath(@"Views\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetPath(@"Views\xslt\books_simple.xslt"));

            string output;
            XrcRequest request = new XrcRequest(new XrcUrl("~/"));
            using (MemoryStream outStream = new MemoryStream())
            {
				XrcResponse response = new XrcResponse(outStream);
	            target.Execute(new Context(request, response));
				response.Flush();

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

            Assert.AreEqual("1-861003-11-0|Franklin,0-201-63361-2|Melville,1-861001-57-6|Plato", output);
        }

		[TestMethod]
		public void Transform_Simple_File()
		{
			var resourceProvider = new Mock<IResourceProviderService>();
			resourceProvider.Setup(p => p.ResourceToXml("a")).Returns(XDocument.Load(TestHelper.GetPath(@"Views\xslt\books.xml")));
			resourceProvider.Setup(p => p.ResourceToXml("b")).Returns(XDocument.Load(TestHelper.GetPath(@"Views\xslt\books_simple.xslt")));

			var page = new Mock<IPage>();
			page.Setup(p => p.GetResourceLocation("a")).Returns("a");
			page.Setup(p => p.GetResourceLocation("b")).Returns("b");

			XsltView target = new XsltView(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null), resourceProvider.Object);
			target.DataFile = "a";
			target.XsltFile = "b";

			string output;
			XrcRequest request = new XrcRequest(new XrcUrl("~/"));
			using (MemoryStream outStream = new MemoryStream())
			{
				XrcResponse response = new XrcResponse(outStream);
				var context = new Context(request, response);
				context.Page = page.Object;

				target.Execute(context);
				response.Flush();

				outStream.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(outStream))
				{
					output = reader.ReadToEnd();
				}
			}

			Assert.AreEqual("1-861003-11-0|Franklin,0-201-63361-2|Melville,1-861001-57-6|Plato", output);
		}

        [TestMethod]
        public void Transform_WithoutData()
        {
			var resourceProvider = new Mock<IResourceProviderService>();
			XsltView target = new XsltView(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null), resourceProvider.Object);
			target.Xslt = XDocument.Load(TestHelper.GetPath(@"Views\xslt\transform_withoutdata.xslt"));

            string output;
			XrcRequest request = new XrcRequest(new XrcUrl("~/"));
			using (MemoryStream outStream = new MemoryStream())
            {
				XrcResponse response = new XrcResponse(outStream);
	            target.Execute(new Context(request, response));
				response.Flush();

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

            Assert.AreEqual("Output from xslt", output);
        }

        [TestMethod]
        public void Transform_Parameter()
        {
			var resourceProvider = new Mock<IResourceProviderService>();
			XsltView target = new XsltView(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null), resourceProvider.Object);
			target.Data = XDocument.Load(TestHelper.GetPath(@"Views\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetPath(@"Views\xslt\parameter.xslt"));

            string output;
			XrcRequest request = new XrcRequest(new XrcUrl("~/"));
			using (MemoryStream outStream = new MemoryStream())
            {
				XrcResponse response = new XrcResponse(outStream);
				Context context = new Context(request, response);
				context.Parameters.Add(new ContextParameter("myParameter", typeof(string), "hello from parameter"));
				target.Execute(context);
				response.Flush();

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

            Assert.AreEqual("hello from parameter", output);
        }

        [TestMethod]
        public void Transform_Module_Extensions()
        {
			var resourceProvider = new Mock<IResourceProviderService>();
			XsltView target = new XsltView(new Mocks.ModuleFactoryMock(new MyExtensionModule()), new Mocks.ModuleCatalogServiceMock(MyExtensionModule.Definition), resourceProvider.Object);
            target.Data = XDocument.Load(TestHelper.GetPath(@"Views\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetPath(@"Views\xslt\extension.xslt"));

            string output;
			XrcRequest request = new XrcRequest(new XrcUrl("~/"));
			using (MemoryStream outStream = new MemoryStream())
            {
				XrcResponse response = new XrcResponse(outStream);
				Context context = new Context(request, response);
				target.Execute(context);

				response.Flush();

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

            Assert.AreEqual("Hello from module", output);
        }

		public class MyExtensionModule
        {
			public static ComponentDefinition Definition = new ComponentDefinition("MyExtensionModule", typeof(MyExtensionModule));

            public string SayHelloFromModule()
            {
                return "Hello from module";
            }
        }
    }
}
