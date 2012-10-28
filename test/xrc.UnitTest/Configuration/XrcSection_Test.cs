using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace xrc.Configuration
{
   
    /// <summary>
    ///This is a test class for SiteConfiguration_Test and is intended
    ///to contain all SiteConfiguration_Test Unit Tests
    ///</summary>
    [TestClass()]
	public class XrcSection_Test
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
			_siteContoso_en.UriPattern = "http://www.contoso.com";
			_siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "paramA", Value = "value_contoso_A" });
			_siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "paramB", Value = "value_contoso_B" });
			_siteContoso_en.Parameters.Add(new SiteParameterElement() { Key = "param2", Value = "override_contoso" });
			_section.Sites.Add(_siteContoso_en);

			_siteContoso_it = new SiteElement();
			_siteContoso_it.Key = "it";
			_siteContoso_it.UriPattern = "http://it.contoso.com";
			_siteContoso_it.Parameters.Add(new SiteParameterElement() { Key = "hello", Value = "ciao" });
			_section.Sites.Add(_siteContoso_it);

			_test = new SiteElement();
			_test.Key = "test";
			_test.UriPattern = "https?://contoso.com:(8080|8043)";
			_test.Parameters.Add(new SiteParameterElement() { Key = "paramA", Value = "8081" });
			_test.Parameters.Add(new SiteParameterElement() { Key = "hello", Value = "hola" });
			_section.Sites.Add(_test);

			_test_virtual = new SiteElement();
			_test_virtual.Key = "test_virtual";
			_test_virtual.UriPattern = "http://contoso.com/virtualpath";
			_section.Sites.Add(_test_virtual);

			_develop = new SiteElement();
			_develop.Key = "develop";
			_develop.UriPattern = "http://localhost";
			_develop.Parameters.Add(new SiteParameterElement() { Key = "debug", Value = "true" });
			_section.Sites.Add(_develop);
		}


        [TestMethod()]
		public void It_should_be_possible_to_read_RootPath_from_configuration_file()
        {
			var target = XrcSection.GetSection();

			Assert.AreEqual("~/sampleWebSiteStructure", target.RootPath.VirtualPath);
		}

		[TestMethod]
		public void It_should_be_possible_to_get_Sites_and_relative_parameters()
		{
			Sites.ISiteConfiguration configuration;

			configuration = GetSiteFromKey(_siteContoso_en.Key);
			Assert.AreEqual(6, configuration.Parameters.Count);
			Assert.AreEqual("false", configuration.Parameters["debug"]);
			Assert.AreEqual("default_1", configuration.Parameters["param1"]);
			Assert.AreEqual("override_contoso", configuration.Parameters["param2"]);
			Assert.AreEqual("hello", configuration.Parameters["hello"]);
			Assert.AreEqual("value_contoso_A", configuration.Parameters["paramA"]);
			Assert.AreEqual(false, configuration.Parameters.ContainsKey("not_existing"));

			configuration = GetSiteFromKey(_siteContoso_it.Key);
			Assert.AreEqual(4, configuration.Parameters.Count);
			Assert.AreEqual("ciao", configuration.Parameters["hello"]);
			Assert.AreEqual("false", configuration.Parameters["debug"]);

			configuration = GetSiteFromKey(_develop.Key);
			Assert.AreEqual(4, configuration.Parameters.Count);
			Assert.AreEqual("hello", configuration.Parameters["hello"]);
			Assert.AreEqual("true", configuration.Parameters["debug"]);
		}

		private Sites.ISiteConfiguration GetSiteFromKey(string key)
		{
			return ((ISitesConfig)_section).Sites.Single(p => p.Key == key);
		}
    }
}
