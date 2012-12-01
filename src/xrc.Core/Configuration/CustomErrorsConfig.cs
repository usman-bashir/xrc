using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using xrc.CustomErrors;

namespace xrc.Configuration
{
	public class CustomErrorsConfig : ICustomErrorsConfig
    {
		readonly List<CustomErrors.ICustomErrorEntry> _customErrors;
		readonly XrcSection _xrcSerction;

		public CustomErrorsConfig(XrcSection xrcSerction)
		{
			_xrcSerction = xrcSerction;
			_customErrors = GetCustomErrors();
		}

		private List<CustomErrors.ICustomErrorEntry> GetCustomErrors()
		{
			var settings = new List<CustomErrors.ICustomErrorEntry>();
			foreach (CustomErrorElement element in _xrcSerction.CustomErrors)
			{
				settings.Add(new CustomErrorEntry(element.StatusCode, element.Url));
			}

			return settings;
		}

		//public IEnumerable<CustomErrors.ICustomErrorEntry> CustomErrors
		//{
		//    get 
		//    {
		//        return _customErrors;
		//    }
		//}

		//public string DefaultUrl
		//{
		//    get { return _xrcSerction.CustomErrors.DefaultUrl; }
		//}

		public string GetErrorPage(int statusCode)
		{
			var customErrorEntry = _customErrors.FirstOrDefault(p => p.StatusCode == statusCode);
			if (customErrorEntry == null)
				return _xrcSerction.CustomErrors.DefaultUrl;

			return customErrorEntry.Url;
		}
	}
}
