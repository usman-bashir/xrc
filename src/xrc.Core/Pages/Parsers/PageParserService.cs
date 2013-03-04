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

namespace xrc.Pages.Parsers
{
    public class PageParserService : IPageParserService
    {
		readonly IResourceParser[] _parsers;

		public PageParserService(IResourceParser[] parsers)
		{
			_parsers = parsers;
		}

        // TODO Parsificare il file una sola volta e metterlo in cache (con dipendenza al file?)

		public PageDefinition Parse(string resourceLocation)
        {
            var parser = _parsers.FirstOrDefault(p => p.CanParse(resourceLocation));
			if (parser == null)
                throw new XrcException(string.Format("Cannot find a parser for '{0}'.", resourceLocation));

            return parser.Parse(resourceLocation);
		}
    }
}
