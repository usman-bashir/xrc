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
		public const string LAYOUT_FILE = "_layout";
		public const string SHARED_FOLDER = "shared";
		public const string FOLDER_CONFIG_FILE = "xrc.config";
		public const string FILE_EXTENSION = ".xrc";
		public const string FILE_PATTERN_STANDARD = "*.xrc";
		public const string FILE_PATTERN_EXTENDED = "*.xrc.*";
		private static Regex _xrcFileRegEx = new Regex(@"^(?<name>.+)\.xrc(\.\w*)?$", RegexOptions.Compiled);
		private static Regex _xrcDirectoryParameterRegEx = new Regex(@"^\{(?<name>.+)\}$", RegexOptions.Compiled);

		public static string GetFileLogicalName(string fileName)
		{
			var match = _xrcFileRegEx.Match(fileName);
			if (!match.Success)
			{
				System.Diagnostics.Debug.Assert(false, string.Format("File '{0}' not valid.", fileName));
				return fileName;
			}

			return match.Groups["name"].Value.ToLowerInvariant();
		}

		public static string GetDirectoryLogicalName(string directoryName)
		{
			return directoryName.ToLowerInvariant();
		}

		public static string GetConfigLogicalName(string configName)
		{
			return configName.ToLowerInvariant();
		}

		public static string GetFileExtension(string fileName)
		{
			return Path.GetExtension(fileName);
		}

		public static string GetDirectoryName(string fullPath)
		{
			// Note: Path.GetDirect

			return new DirectoryInfo(fullPath).Name;
		}

		//public static string GetDirectoryParameterName(string directoryName)
		//{
		//    var match = _xrcDirectoryParameterRegEx.Match(directoryName);
		//    if (!match.Success)
		//        return null;

		//    return match.Groups["name"].Value;
		//}
	}
}
