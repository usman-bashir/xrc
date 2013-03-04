using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Parsers
{
	public interface IResourceParser
	{
		string Extension { get; }

		bool CanParse(string resourceLocation);

		PageDefinition Parse(string resourceLocation);
	}
}
