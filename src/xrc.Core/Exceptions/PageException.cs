using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    [Serializable]
    public class PageException : XrcException
    {
        public PageException(string url, Exception innerException)
			: base(string.Format("Failed to execute '{0}'. '{1}'", url, innerException.Message), innerException) 
        {
			Url = url;
        }

		public string Url
        {
            get;
            private set;
        }

		protected PageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
