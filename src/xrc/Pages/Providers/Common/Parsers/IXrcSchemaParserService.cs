using System;
namespace xrc.Pages.Providers.Common.Parsers
{
	public interface IXrcSchemaParserService
	{
		PageParserResult Parse(XrcItem item);
	}
}
