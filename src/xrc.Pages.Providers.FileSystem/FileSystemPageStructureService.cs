using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Configuration;
using System.IO;
using xrc.Pages.TreeStructure;
using xrc.Pages.Parsers;

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

        public PageRoot GetRoot()
		{
			// TODO Mettere in cache questo valore (dipendenza a file?) o gestirlo a livello di classe (FileSystemWatcher?)

			var root = new PageRoot(_config.XrcRootVirtualPath);

            FillItems(root);

			return root;
		}

        void FillItems(PageDirectory directory)
		{
            string directoryPath = _config.MapPath(directory.ResourceLocation);

			var directories = Directory.GetDirectories(directoryPath)
                                        .Select(p => new PageDirectory(Path.GetFileName(p)));
			directory.Directories.AddRange(directories);

            foreach (PageDirectory subItem in directory.Directories)
                FillItems(subItem);

			foreach (var parser in _parsers)
			{
				var parserFiles = Directory.GetFiles(directoryPath, string.Format("*{0}", parser.Extension))
												.Select(p => new PageFile(Path.GetFileName(p)));
                directory.Files.AddRange(parserFiles);
			}

			var configFiles = Directory.GetFiles(directoryPath, ConfigFile.XRC_DIRECTORY_CONFIG_FILE)
											.Select(p => new ConfigFile());
            directory.ConfigFile = configFiles.FirstOrDefault();
		}
	}
}
