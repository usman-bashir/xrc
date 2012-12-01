using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using xrc.CustomErrors;

namespace xrc.Configuration
{
	/// <summary>
	/// Implements IEnvironmentConfig interface reading Settings from ConfigurationManager.AppSettings (web.config file).
	/// </summary>
	public class EnvironmentConfig : IEnvironmentConfig
    {
		Dictionary<string, object> _settings;

		public EnvironmentConfig()
		{
			_settings = GetSettings();
		}

		private Dictionary<string, object> GetSettings()
		{
			var settings = new Dictionary<string, object>();
			foreach (var key in ConfigurationManager.AppSettings.AllKeys)
				settings.Add(key, ConfigurationManager.AppSettings[key]);

			return settings;
		}

		public Dictionary<string, object> Settings
		{
			get 
			{
				return _settings;
			}
		}
	}
}
