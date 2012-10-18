using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common.Parsers
{
	public class XrcParserService : ParserServiceBase
	{
		readonly IXrcSchemaParserService _xrcSchemaParser;

		public XrcParserService(IXrcSchemaParserService configParser, IXrcSchemaParserService xrcSchemaParser)
			: base(configParser, ".xrc")
		{
			_xrcSchemaParser = xrcSchemaParser;
		}

		protected override PageParserResult ParseFile(XrcItem item)
		{
			var result = _xrcSchemaParser.Parse(item);

			foreach (var action in result.Actions)
			{
				if (action.Layout == null)
					action.Layout = GetDefaultLayoutByConvention(item);
			}

			return result;
		}
	}
}
