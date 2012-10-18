using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages.Providers.Common
{
	public interface IParserService
	{
		string Extension { get; }

		bool CanParse(XrcItem file);

		PageParserResult Parse(XrcItem file);
	}
}
