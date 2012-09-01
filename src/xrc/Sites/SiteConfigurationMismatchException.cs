using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Sites
{
	[Serializable]
	public class SiteConfigurationMismatchException : XrcException
	{
		public SiteConfigurationMismatchException() { }
		public SiteConfigurationMismatchException(string message) : base(message) { }
		public SiteConfigurationMismatchException(string message, Exception inner) : base(message, inner) { }
		protected SiteConfigurationMismatchException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
