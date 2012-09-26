using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public class XrcParserService : ParserServiceBase
	{
		private IXrcSchemaParserService _xrcSchemaParser;
		public XrcParserService(IXrcSchemaParserService configParser, IXrcSchemaParserService xrcSchemaParser)
			: base(configParser, ".xrc")
		{
			_xrcSchemaParser = xrcSchemaParser;
		}

		protected override PageParserResult ParseFile(XrcFileResource fileResource)
		{
			var result = _xrcSchemaParser.Parse(fileResource.File.FullPath);

			foreach (var action in result.Actions)
			{
				if (action.Layout == null)
					action.Layout = GetDefaultLayoutByConvention(fileResource);
			}

			return result;
		}
	}
}
