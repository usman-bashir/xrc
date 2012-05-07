using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace xrc.Configuration
{
    [TestClass]
    public class SiteConfigurationProviderService_Test
    {
        private XrcSection _section;
        private SiteElement _siteContoso_en;
        private SiteElement _siteContoso_it;
        private SiteElement _test;
        private SiteElement _test_virtual;
        private SiteElement _develop;

        [TestInitialize]
        public void TestInitialize()
        {
            _section = new XrcSection();

            _section.Parameters.Add(new SiteParameterElement() { Key = "param1", Value = "default_1" });
            _section.Parameters.Add(new SiteParameterElement() { Key = "param2", Value = "default_2" });
            _section.Parameters.Add(new SiteParameterElement() { Key = "hello", Value = "hello" });
            _section.Parameters.Add(new SiteParameterElement() { Key = "debug", Value = "false" });

            _siteContoso_en = new SiteElement();
            _siteContoso_en.Key = "en";
            _siteContoso_en.Uri = new Uri("http://www.contoso.com");
            _siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "paramA", Value = "value_contoso_A" });
            _siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "paramB", Value = "value_contoso_B" });
            _siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "param2", Value = "override_contoso" });
            _section.Sites.Add(_siteContoso_en);

            _siteContoso_it = new SiteElement();
            _siteContoso_it.Key = "it";
            _siteContoso_it.Uri = new Uri("http://it.contoso.com");
            _siteContoso_it.Parameters.Add(new SiteParameterElement() { Key = "hello", Value = "ciao" });
            _section.Sites.Add(_siteContoso_it);

            _test = new SiteElement();
            _test.Key = "test";
            _test.Uri = new Uri("http://contoso.com:8080");
            _test.SecureUri = new Uri("https://contoso.com:8043");
            _test.Parameters.Add(new SiteParameterElement() { Key = "paramA", Value = "8081" });
            _test.Parameters.Add(new SiteParameterElement() { Key = "hello", Value = "hola" });
            _section.Sites.Add(_test);

            _test_virtual = new SiteElement();
            _test_virtual.Key = "test_virtual";
            _test_virtual.Uri = new Uri("http://contoso.com/virtualpath");
            _section.Sites.Add(_test_virtual);

            _develop = new SiteElement();
            _develop.Key = "develop";
            _develop.Uri = new Uri("http://localhost");
            _develop.Parameters.Add(new SiteParameterElement() { Key = "debug", Value = "true" });
            _section.Sites.Add(_develop);
        }

        [TestMethod]
        public void It_should_be_possible_to_GetSiteFromUri()
        {
            SiteConfigurationProviderService target = new SiteConfigurationProviderService(_section);
            ISiteConfiguration configuration;

            // _siteContoso_en
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("http://www.CONTOSO.com/DEMO"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("https://www.contoso.com:443"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com/"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com:80/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);
            configuration = target.GetSiteFromUri(new Uri("https://www.contoso.com:443/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _siteContoso_en.Key);

            // _siteContoso_it
            configuration = target.GetSiteFromUri(new Uri("http://it.contoso.com"));
            Assert.AreEqual(configuration.Key, _siteContoso_it.Key);
            configuration = target.GetSiteFromUri(new Uri("http://it.contoso.com:80/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _siteContoso_it.Key);

            // _test
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com:8080"));
            Assert.AreEqual(configuration.Key, _test.Key);
            configuration = target.GetSiteFromUri(new Uri("https://contoso.com:8043/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _test.Key);

            // _test_virtual
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com/virtualpath"));
            Assert.AreEqual(configuration.Key, _test_virtual.Key);
            configuration = target.GetSiteFromUri(new Uri("http://contoso.COM/VIRTUALPATH"));
            Assert.AreEqual(configuration.Key, _test_virtual.Key);
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com/virtualpath/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, _test_virtual.Key);

            // invalid uri
            TestHelper.Throws<ApplicationException>(() => target.GetSiteFromUri(new Uri("http://contoso.com"))); // In this case you probably need a redirect to www.contoso.com
            TestHelper.Throws<ApplicationException>(() => target.GetSiteFromUri(new Uri("http://anothersite.it")));
            TestHelper.Throws<ApplicationException>(() => target.GetSiteFromUri(new Uri("http://contoso")));
            TestHelper.Throws<ApplicationException>(() => target.GetSiteFromUri(new Uri("http://contoso.com:8043")));
            TestHelper.Throws<ApplicationException>(() => target.GetSiteFromUri(new Uri("http://www.contoso.com:443")));
        }

        [TestMethod]
        public void It_should_be_possible_to_GetParameters()
        {
            SiteConfigurationProviderService target = new SiteConfigurationProviderService(_section);
            ISiteConfiguration configuration;

            configuration = target.GetSiteFromKey(_siteContoso_en.Key);
            Assert.AreEqual(6, configuration.Parameters.Count);
            Assert.AreEqual("false", configuration.Parameters["debug"]);
            Assert.AreEqual("default_1", configuration.Parameters["param1"]);
            Assert.AreEqual("override_contoso", configuration.Parameters["param2"]);
            Assert.AreEqual("hello", configuration.Parameters["hello"]);
            Assert.AreEqual("value_contoso_A", configuration.Parameters["paramA"]);
            Assert.AreEqual(false, configuration.Parameters.ContainsKey("not_existing"));

            configuration = target.GetSiteFromKey(_siteContoso_it.Key);
            Assert.AreEqual(4, configuration.Parameters.Count);
            Assert.AreEqual("ciao", configuration.Parameters["hello"]);
            Assert.AreEqual("false", configuration.Parameters["debug"]);

            configuration = target.GetSiteFromKey(_develop.Key);
            Assert.AreEqual(4, configuration.Parameters.Count);
            Assert.AreEqual("hello", configuration.Parameters["hello"]);
            Assert.AreEqual("true", configuration.Parameters["debug"]);
        }

        [TestMethod]
        public void It_should_be_possible_to_read_configuration_file()
        {
            // see App.config
            SiteConfigurationProviderService target = new SiteConfigurationProviderService(XrcSection.GetSection());
            ISiteConfiguration configuration;

            configuration = target.GetSiteFromUri(new Uri("http://www.northwind.com"));
            Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("https://www.northwind.com:443"));
            Assert.AreEqual(configuration.Key, "en");
            Assert.AreEqual("false", configuration.Parameters["debug"]);
            Assert.AreEqual("en", configuration.Parameters["culture"]);

            configuration = target.GetSiteFromUri(new Uri("http://it.northwind.com"));
            Assert.AreEqual(configuration.Key, "it");
            configuration = target.GetSiteFromUri(new Uri("http://it.northwind.com:80/test?prova=field#ciao"));
            Assert.AreEqual(configuration.Key, "it");
            Assert.AreEqual("false", configuration.Parameters["debug"]);
            Assert.AreEqual("it", configuration.Parameters["culture"]);

            configuration = target.GetSiteFromUri(new Uri("http://localhost:8080"));
            Assert.AreEqual(configuration.Key, "local");
            configuration = target.GetSiteFromUri(new Uri("https://localhost:8043"));
            Assert.AreEqual(configuration.Key, "local");
            Assert.AreEqual("true", configuration.Parameters["debug"]);
            Assert.AreEqual("en", configuration.Parameters["culture"]);
        }
    }
}
