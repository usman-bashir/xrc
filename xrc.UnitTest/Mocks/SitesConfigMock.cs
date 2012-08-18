using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using xrc.Configuration;

namespace xrc.Mocks
{
	public class SitesConfigMock : ISitesConfig
	{
		private IEnumerable<Sites.ISiteConfiguration> _sites;
		public SitesConfigMock(params Sites.ISiteConfiguration[] configs)
		{
			_sites = configs;
		}

		public IEnumerable<Sites.ISiteConfiguration> Sites
		{
			get { return _sites; }
		}
	}
}
