using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace xrc.Configuration
{
	public interface ICustomErrorsConfig
    {
		string GetErrorPage(int statusCode);
	}
}
