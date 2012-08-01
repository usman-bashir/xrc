using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    [Serializable]
    public class ResourceNotFoundException : XrcException
    {
        public ResourceNotFoundException(string resource) 
            : base(string.Format("Resource '{0}' not found.", resource)) { }
        protected ResourceNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
