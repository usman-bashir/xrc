using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.FileSystem.Parsers
{
	public interface IParserService
	{
		bool CanParse(XrcFile file);

		PageParserResult Parse(XrcFileResource fileResource);
	}
}
