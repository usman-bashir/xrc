using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    [Serializable]
    public class MovedPermanentlyException : XrcException
    {
        public MovedPermanentlyException(string resource, string newUrl) 
            : base(string.Format("Resource '{0}' moved permanently to '{1}'.", resource, newUrl)) 
        {
            NewUrl = newUrl;
        }

        public string NewUrl
        {
            get;
            private set;
        }

        protected MovedPermanentlyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
