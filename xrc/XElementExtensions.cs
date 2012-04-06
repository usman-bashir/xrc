using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc
{
	public static class XElementExtensions
	{
		public static T AttributeAs<T>(this XElement element, XName name)
		{
			var a = element.Attribute(name);
			if (a == null)
				throw new ApplicationException(string.Format("Attribute '{0}' not found.", name));
			else
				return (T)Convert.ChangeType(a.Value, typeof(T),
											System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
