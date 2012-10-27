using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
	public interface IHostingConfig
    {
		Uri WebSiteVirtualDirectory
		{
			get;
		}

		/// <summary>
		/// Converts an application relative url (starting with ~) with a domain relative url. Example: ~/test/index became /site/test/index.
		/// </summary>
		Uri AppRelativeUrlToRelativeUrl(string url);

		string RelativeUrlToAppRelativeUrl(Uri url);
	}

	// TODO Da spostare nella sezione file system

	public interface IFileSystemConfig
	{
		string XrcRootVirtualPath
		{
			get;
		}

		string MapPath(string virtualPath);
	}
}
