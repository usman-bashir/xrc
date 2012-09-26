using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Script;
using xrc.Modules;
using xrc.Views;
using System.Xml.Linq;
using System.Reflection;

namespace xrc.Pages.Providers.FileSystem.Parsers
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

		public bool CanParse(XrcFile file)
		{
			return string.Equals(file.Extension, _extension, StringComparison.InvariantCultureIgnoreCase);
		}

		public PageParserResult Parse(XrcFileResource fileResource)
		{
			var parserResult = ParseConfigFiles(fileResource);

			var fileResult = ParseFile(fileResource);

			return parserResult.Union(fileResult);
		}

		private PageParserResult ParseConfigFiles(XrcFileResource fileResource)
		{
			PageParserResult result = new PageParserResult();
			string[] configFiles = GetFolderConfigFiles(fileResource);
			foreach (var f in configFiles)
			{
				var configResult = _configParser.Parse(f);

				result = result.Union(configResult);
			}

			return result;
		}

		private string[] GetFolderConfigFiles(XrcFileResource fileResource)
		{
			var configFiles = new List<string>();

			XrcFolder currentFolder = fileResource.File.Parent;
			while (currentFolder != null)
			{
				string f = currentFolder.GetConfigFile();
				if (f != null)
					configFiles.Add(f);

				currentFolder = currentFolder.Parent;
			}

			configFiles.Reverse();
			return configFiles.ToArray();
		}

		protected string GetDefaultLayoutByConvention(XrcFileResource fileResource)
		{
			if (!fileResource.File.IsSlot())
				return SearchParent(fileResource.File.Parent);

			return null;
		}

		private string SearchParent(XrcFolder folder)
		{
			var layoutFile = folder.SearchLayout();
			if (layoutFile != null)
				return layoutFile.FullName;

			return null;
		}

		protected abstract PageParserResult ParseFile(XrcFileResource fileResource);
	}
}
