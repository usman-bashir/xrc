using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace xrc.Pages.Providers.FileSystem
{
	// TODO Valutare se trasformare questa classe in un servizio

    public static class XrcFileSystemHelper
    {
		public const string INDEX_FILE = "index";
		public const string FOLDER_CONFIG_FILE = "xrc.config";
		public const string FILE_EXTENSION = ".xrc";
		public const string FILE_PATTERN = "*" + FILE_EXTENSION;
		private static Regex _xrcFileRegEx = new Regex(@"^(?<name>.+)\.xrc(\.\w*)?$", RegexOptions.Compiled);
		private static Regex _xrcDirectoryParameterRegEx = new Regex(@"^\{(?<name>.+)\}$", RegexOptions.Compiled);

		public static string GetFileName(string fullPath)
		{
			string fileName = Path.GetFileName(fullPath);

			var match = _xrcFileRegEx.Match(fileName);
			if (!match.Success)
			{
				System.Diagnostics.Debug.Assert(false, string.Format("File '{0}' not valid.", fullPath));
				return fileName;
			}

			return match.Groups["name"].Value;
		}

		public static string GetFileExtension(string fullPath)
		{
			return Path.GetExtension(fullPath);
		}

		public static string GetDirectoryName(string fullPath)
		{
			return new DirectoryInfo(fullPath).Name;
		}

		public static string GetDirectoryParameterName(string directoryName)
		{
			var match = _xrcDirectoryParameterRegEx.Match(directoryName);
			if (!match.Success)
				return null;

			return match.Groups["name"].Value;
		}
    }
}
