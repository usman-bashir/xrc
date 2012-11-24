using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Providers.Common;
using xrc.Configuration;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageStructureService : IPageStructureService
    {
		readonly IFileSystemConfig _config;
		readonly IParserService[] _parsers;

		public FileSystemPageStructureService(IFileSystemConfig config, IParserService[] parsers)
		{
			_config = config;
			_parsers = parsers;
		}

		public XrcItem GetRoot()
		{
			// TODO Mettere in cache questo valore (dipendenza a file?) o gestirlo a livello di classe (FileSystemWatcher?)

			var root = XrcItem.NewRoot(_config.XrcRootVirtualPath);
			FillItems(root);

			return root;
		}

		void FillItems(XrcItem item)
		{
			if (item.ItemType != XrcItemType.Directory)
				return;

			string directoryPath = _config.MapPath(item.ResourceLocation);

			var directories = Directory.GetDirectories(directoryPath)
										.Select(p => XrcItem.NewDirectory(Path.GetFileName(p)));
			item.Items.AddRange(directories);

			foreach (var parser in _parsers)
			{
				var parserFiles = Directory.GetFiles(directoryPath, string.Format("*{0}", parser.Extension))
												.Select(p => XrcItem.NewXrcFile(Path.GetFileName(p)));
				item.Items.AddRange(parserFiles);
			}

			var configFiles = Directory.GetFiles(directoryPath, XrcItem.XRC_DIRECTORY_CONFIG_FILE)
											.Select(p => XrcItem.NewConfigFile());
			item.Items.AddRange(configFiles);

			foreach (var subItem in item.Items)
				FillItems(subItem);
		}
	}
}
