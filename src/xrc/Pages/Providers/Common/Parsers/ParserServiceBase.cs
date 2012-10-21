using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Views;
using System.Xml.Linq;
using System.Reflection;

namespace xrc.Pages.Providers.Common.Parsers
{
	public abstract class ParserServiceBase : IParserService
	{
		readonly string _extension;
		readonly IXrcSchemaParserService _configParser;

		public ParserServiceBase(IXrcSchemaParserService configParser, string extension)
		{
			_extension = extension;
			_configParser = configParser;
		}

		public string Extension
		{
			get { return _extension; }
		}

		public bool CanParse(XrcItem file)
		{
			return file.ResourceName.EndsWith(_extension, StringComparison.InvariantCultureIgnoreCase);
		}

		public PageParserResult Parse(XrcItem item)
		{
			var parserResult = ParseConfigFiles(item);

			var fileResult = ParseFile(item);

			return parserResult.Union(fileResult);
		}

		private PageParserResult ParseConfigFiles(XrcItem item)
		{
			PageParserResult result = new PageParserResult();
			XrcItem[] configFiles = GetFolderConfigFiles(item);
			foreach (var f in configFiles)
			{
				var configResult = _configParser.Parse(f);

				result = result.Union(configResult);
			}

			return result;
		}

		private XrcItem[] GetFolderConfigFiles(XrcItem item)
		{
			var configFiles = new List<XrcItem>();

			XrcItem currentFolder = item.Parent;
			while (currentFolder != null)
			{
				XrcItem f = currentFolder.ConfigFile;
				if (f != null)
					configFiles.Add(f);

				currentFolder = currentFolder.Parent;
			}

			configFiles.Reverse();
			return configFiles.ToArray();
		}

		protected string GetDefaultLayoutByConvention(XrcItem item)
		{
			if (!item.IsSlot)
				return GetLayout(item.Parent);

			return null;
		}

		private string GetLayout(XrcItem item)
		{
			var layoutFile = item.LayoutFile;
			if (layoutFile != null)
				return layoutFile.GetUrl().ToString();

			return null;
		}

		protected abstract PageParserResult ParseFile(XrcItem fileResource);
	}
}
