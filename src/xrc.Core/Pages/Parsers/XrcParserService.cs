using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Parsers
{
	public class XrcParserService : ParserServiceBase
	{
		readonly XrcSchemaParserService _xrcSchemaParser;

        public XrcParserService(XrcSchemaParserService xrcSchemaParser)
			: base(".xrc")
		{
			_xrcSchemaParser = xrcSchemaParser;
		}

		public override PageDefinition Parse(string resourceLocation)
		{
            var result = _xrcSchemaParser.Parse(resourceLocation);

			return result;
		}
	}
}
