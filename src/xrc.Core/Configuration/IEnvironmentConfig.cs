using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace xrc.Configuration
{
	public interface IEnvironmentConfig
    {
		Dictionary<string, object> Settings { get; }
	}
}
