using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace xrc.Sites
{
    [TestClass]
    public class SiteConfigurationProviderService_Test
    {
        [TestMethod]
        public void It_should_be_possible_to_GetSiteFromUri()
        {
			var sitesConfig = new Mocks.SitesConfigMock(
					new SiteConfiguration("en", new Uri("http://www.contoso.com")),
					new SiteConfiguration("it", new Uri("http://it.contoso.com")),
					new SiteConfiguration("test", new Uri("http://contoso.com:8080"), null, new Uri("https://contoso.com:8043")),
					new SiteConfiguration("test_virtual", new Uri("http://contoso.com/virtualpath"))
				);

			SiteConfigurationProviderService target = new SiteConfigurationProviderService(sitesConfig);
            ISiteConfiguration configuration;

            // _siteContoso_en
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com"));
            Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("http://www.CONTOSO.com/DEMO"));
			Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("https://www.contoso.com:443"));
			Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com/"));
			Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("http://www.contoso.com:80/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "en");
            configuration = target.GetSiteFromUri(new Uri("https://www.contoso.com:443/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "en");

            // _siteContoso_it
            configuration = target.GetSiteFromUri(new Uri("http://it.contoso.com"));
            Assert.AreEqual(configuration.Key, "it");
            configuration = target.GetSiteFromUri(new Uri("http://it.contoso.com:80/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "it");

            // _test
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com:8080"));
            Assert.AreEqual(configuration.Key, "test");
            configuration = target.GetSiteFromUri(new Uri("https://contoso.com:8043/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "test");

            // _test_virtual
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com/virtualpath"));
            Assert.AreEqual(configuration.Key, "test_virtual");
            configuration = target.GetSiteFromUri(new Uri("http://contoso.COM/VIRTUALPATH"));
			Assert.AreEqual(configuration.Key, "test_virtual");
            configuration = target.GetSiteFromUri(new Uri("http://contoso.com/virtualpath/test?prova=field#ciao"));
			Assert.AreEqual(configuration.Key, "test_virtual");

            // invalid uri
            TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://contoso.com"))); // In this case you probably need a redirect to www.contoso.com
			TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://anothersite.it")));
			TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://contoso")));
			TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://contoso.com:8043")));
			TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://www.contoso.com:443")));
        }

        [TestMethod]
		public void It_should_be_possible_to_GetSiteFromUri_from_configuration_file()
        {
            // see App.config
            SiteConfigurationProviderService target = new SiteConfigurationProviderService(xrc.Configuration.XrcSection.GetSection());
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

			// invalid uri
			TestHelper.Throws<SiteConfigurationNotFoundException>(() => target.GetSiteFromUri(new Uri("http://anothersite.it")));
        }
    }
}
