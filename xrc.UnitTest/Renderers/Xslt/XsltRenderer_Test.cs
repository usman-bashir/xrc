﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace xrc.Renderers
{
    [TestClass]
    public class XsltRenderer_Test
    {
        [TestMethod]
        public void Transform_Simple()
        {
            XsltRenderer target = new XsltRenderer(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null));
			target.Data = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\books_simple.xslt"));

            string output;
            XrcRequest request = new XrcRequest(new Uri("http://test/"));
            using (MemoryStream outStream = new MemoryStream())
            {
                XrcResponse response = new XrcResponse(outStream);

                target.RenderRequest(new Context(request, response));

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
            XsltRenderer target = new XsltRenderer(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null));
            target.Xslt = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\transform_withoutdata.xslt"));

            string output;
            XrcRequest request = new XrcRequest(new Uri("http://test/"));
            using (MemoryStream outStream = new MemoryStream())
            {
                XrcResponse response = new XrcResponse(outStream);

                target.RenderRequest(new Context(request, response));

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
            XsltRenderer target = new XsltRenderer(new Mocks.ModuleFactoryMock(null), new Mocks.ModuleCatalogServiceMock(null));
            target.Data = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\parameter.xslt"));

            string output;
            XrcRequest request = new XrcRequest(new Uri("http://test/"));
            using (MemoryStream outStream = new MemoryStream())
            {
                XrcResponse response = new XrcResponse(outStream);
                Context context = new Context(request, response);
                context.Parameters.Add("myParameter", "hello from parameter");
                target.RenderRequest(context);

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
            XsltRenderer target = new XsltRenderer(new Mocks.ModuleFactoryMock(new MyModuleExtension()), new Mocks.ModuleCatalogServiceMock(MyModuleExtension.Definition));
            target.Data = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\books.xml"));
            target.Xslt = XDocument.Load(TestHelper.GetFile(@"Renderers\xslt\extension.xslt"));

            string output;
            XrcRequest request = new XrcRequest(new Uri("http://test/"));
            using (MemoryStream outStream = new MemoryStream())
            {
                XrcResponse response = new XrcResponse(outStream);
                Context context = new Context(request, response);
                target.RenderRequest(context);

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

            Assert.AreEqual("Hello from module", output);
        }

        public class MyModuleExtension : xrc.Modules.IModule
        {
            public static ComponentDefinition Definition = new ComponentDefinition("MyModuleExt", typeof(MyModuleExtension));

            public string SayHelloFromModule()
            {
                return "Hello from module";
            }
        }
    }
}