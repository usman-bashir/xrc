using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Configuration;
using System.Reflection;
using xrc.Script;
using System.Globalization;
using xrc.Views;
using xrc.Modules;
using xrc.Pages.Script;
using xrc.Pages.Providers.FileSystem.Parsers;

namespace xrc.Pages.Providers.FileSystem
{
    public class PageParserService : IPageParserService
    {
		readonly IParserService[] _parsers;

		public PageParserService(IParserService[] parsers)
		{
			_parsers = parsers;
		}

        // TODO Parsificare il file una sola volta e metterlo in cache (con dipendenza al file?)

		public PageParserResult Parse(XrcFileResource fileResource)
        {
			var parser = _parsers.FirstOrDefault(p => p.CanParse(fileResource.File));
			if (parser == null)
				throw new XrcException(string.Format("Cannot find a parser for file '{0}'.", fileResource.File.FullPath));

			return parser.Parse(fileResource);
		}
    }
}
