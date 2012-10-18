using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages.Providers.Common;
using xrc.Configuration;
using System.IO;
using xrc.Pages.Providers.Common;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageStructureService : IPageStructureService
    {
		readonly IRootPathConfig _rootPath;
		readonly IParserService[] _parsers;

		public FileSystemPageStructureService(IRootPathConfig rootPath, IParserService[] parsers)
		{
			_rootPath = rootPath;
			_parsers = parsers;
		}

		public XrcItem GetRoot()
		{
			// TODO Mettere in cache questo valore (dipendenza a file?) o gestirlo a livello di classe (FileSystemWatcher?)

			var root = new XrcItem(null, XrcItemType.Directory, _rootPath.PhysicalPath, GetDirectoryName());
			FillItems(root);

			return root;
		}

		void FillItems(XrcItem item)
		{
			if (item.ItemType != XrcItemType.Directory)
				return;

			var directories = Directory.GetDirectories(item.Id)
										.Select(p => new XrcItem(item, XrcItemType.Directory, p, Path.GetFileName(p)));
			item.Items.AddRange(directories);

			foreach (var parser in _parsers)
			{
				var parserFiles = Directory.GetFiles(item.Id, string.Format("*.{0}", parser.Extension))
												.Select(p => new XrcItem(item, XrcItemType.XrcFile, p, Path.GetFileName(p)));
				item.Items.AddRange(parserFiles);
			}

			var configFiles = Directory.GetFiles(item.Id, XrcItem.XRC_DIRECTORY_CONFIG_FILE)
											.Select(p => new XrcItem(item, XrcItemType.ConfigFile, p, Path.GetFileName(p)));
			item.Items.AddRange(configFiles);

			foreach (var subItem in item.Items)
				FillItems(subItem);
		}

		string GetDirectoryName()
		{
			return Path.GetFileName(_rootPath.PhysicalPath);
		}
	}
}
