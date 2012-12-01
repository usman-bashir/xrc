using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace xrc.Configuration
{
    [TestClass()]
	public class XrcSection_Test
    {
		private XrcSection _section;

		[TestInitialize]
		public void TestInitialize()
		{
			_section = new XrcSection();

		}

        [TestMethod()]
		public void It_should_be_possible_to_read_RootPath_from_configuration_file()
        {
			var target = XrcSection.GetSection();

			Assert.AreEqual("~/sampleWebSiteStructure", target.RootPath.VirtualPath);
		}
    }
}
