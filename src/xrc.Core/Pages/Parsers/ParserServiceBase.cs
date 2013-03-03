using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Views;
using System.Xml.Linq;
using System.Reflection;

namespace xrc.Pages.Parsers
{
	public abstract class ParserServiceBase : IParserService
	{
		readonly string _extension;

		protected ParserServiceBase(string extension)
		{
			_extension = extension;
		}

		public string Extension
		{
			get { return _extension; }
		}

		public bool CanParse(string resourceLocation)
		{
            return resourceLocation.EndsWith(_extension, StringComparison.InvariantCultureIgnoreCase);
		}

		public abstract PageDefinition Parse(string resourceLocation);
	}
}
