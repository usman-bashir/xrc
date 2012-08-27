using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Sites
{
	[Serializable]
	public class SiteConfigurationNotFoundException : XrcException
	{
		public SiteConfigurationNotFoundException() { }
		public SiteConfigurationNotFoundException(string message) : base(message) { }
		public SiteConfigurationNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected SiteConfigurationNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
