using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using xrc.Modules;
using xrc.Pages.Providers;
using xrc.Pages.Script;
using xrc.Views;

namespace xrc.Pages.Parsers
{
	public class XrcConfigParser : ResourceParserBase
	{
        readonly XrcParser _xrcParser;
        public XrcConfigParser(XrcParser xrcParser)
			: base("xrc.config")
		{
            _xrcParser = xrcParser;
		}

		public override PageDefinition Parse(string resourceLocation)
		{
            return _xrcParser.Parse(resourceLocation);
		}
	}
}
