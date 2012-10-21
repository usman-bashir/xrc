using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Configuration
{
#warning probabilmente questa interfaccia è da dividere in due, una che gestisce i dati web (virtual Path) e l'altra che gestisce i dati del file system (MapPath, PhysicalPath da trasformare in ResourceLocationBase?)

	public interface IRootPathConfig
    {
        string VirtualPath
        {
            get;
        }

        string PhysicalPath
        {
            get;
        }

		string MapPath(string virtualPath);

		/// <summary>
		/// Converts an application relative url (starting with ~) with a domain relative url. Example: ~/test/index became /site/test/index.
		/// </summary>
		Uri AppRelativeUrlToRelativeUrl(string url);
	}
}
