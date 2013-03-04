using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.Linq;

namespace xrc.Pages.Providers
{
	public class FileSystemResourceProviderService : IResourceProviderService
	{
		readonly Configuration.IFileSystemConfig _config;

		public FileSystemResourceProviderService(Configuration.IFileSystemConfig config)
		{
			_config = config;
		}

		// TODO Valutare come e se fare cache del risultato di GetPage e IsDefined anche perchè condividono parte del codice.
		// Probabilmente si può mettere in cache l'intera IPage con dipendenza a xrcFile.File.FullPath

		public bool ResourceExists(string resourceLocation)
		{
			string file = MapPath(resourceLocation);
			return File.Exists(file);
		}

		public Stream OpenResource(string resourceLocation)
		{
			string filePath = MapPath(resourceLocation);

			return File.OpenRead(filePath);
		}

		public XDocument ResourceToXml(string resourceLocation)
		{
			using (Stream stream = OpenResource(resourceLocation))
			{
				return XDocument.Load(stream);
			}
		}

		public string ResourceToText(string resourceLocation)
		{
			using (StreamReader stream = new StreamReader(OpenResource(resourceLocation), true))
			{
				string text = stream.ReadToEnd();
				stream.Close();

				return text;
			}
		}

		public byte[] ResourceToBytes(string resourceLocation)
		{
			using (var memStream = new MemoryStream())
			{
				using (var resStream = OpenResource(resourceLocation))
				{
					resStream.CopyTo(memStream);
				}
				return memStream.ToArray();
			}
		}

		public XDocument ResourceToXHtml(string resourceLocation)
		{
			return ResourceToXml(resourceLocation);
		}

		public string ResourceToHtml(string resourceLocation)
		{
			return ResourceToText(resourceLocation);
		}

		public string ResourceToJson(string resourceLocation)
		{
            return ResourceToText(resourceLocation);
		}

		private string MapPath(string resourceLocation)
		{
			string filePath;
			if (Path.IsPathRooted(resourceLocation))
				filePath = resourceLocation;
			else
				filePath = _config.MapPath(resourceLocation);
			return filePath;
		}
	}
}
