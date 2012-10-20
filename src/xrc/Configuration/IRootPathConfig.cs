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

		string RelativeUrlToVirtual(Uri relativeUrl);
		Uri VirtualUrlToRelative(string virtualUrl);
	}
}
