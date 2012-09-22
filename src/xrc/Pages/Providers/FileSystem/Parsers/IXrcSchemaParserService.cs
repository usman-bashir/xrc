using System;
namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public interface IXrcSchemaParserService
	{
		PageParserResult Parse(string fullpath);
	}
}
