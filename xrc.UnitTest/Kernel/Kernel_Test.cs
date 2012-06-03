using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xrc.SiteManager;
using xrc.Configuration;
using Moq;

namespace xrc
{
    [TestClass]
    public class Kernel_Test
    {
        //private CmsPageDefinition _pageDefinition;
        //private SiteConfiguration _siteConfiguration;
        //private CmsFile _cmsFile;

        //private Mock<ISiteManagerService> _parser;
        //private Mock<ISiteConfigurationProviderService> _siteConfigurationProvider;
        //private Mock<ICmsFileLocatorService> _fileLocator;
        //private string _workingPath;

        //[TestInitialize]
        //public void Init()
        //{
        //    _siteConfiguration = new SiteConfiguration("test", new Uri("http://contoso.com"), new Dictionary<string, string>());
        //    _pageDefinition = new CmsPageDefinition();
        //    _cmsFile = new CmsFile("test.xrc", new Dictionary<string, string>());

        //    _parser = new Mock<ISiteManagerService>();
        //    _parser.Setup(p => p.Parse(It.IsAny<string>(), It.IsAny<Module[]>())).Returns(_pageDefinition);

        //    _siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
        //    _siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(_siteConfiguration);

        //    _fileLocator = new Mock<ICmsFileLocatorService>();
        //    _fileLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns(_cmsFile);
        //}

        // TODO Rivedere architettura per rendere il Kernel testabile...il problema è che non dovrebbe avere il container...

        //[TestMethod]
        //public void It_should_be_possible_to_process_request()
        //{
        //    var target = new Kernel("");
        //}
    }
}
