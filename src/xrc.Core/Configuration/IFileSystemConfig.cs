using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
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
