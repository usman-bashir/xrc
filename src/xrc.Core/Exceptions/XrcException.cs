using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    [Serializable]
    public class XrcException : Exception
    {
        public XrcException() { }
        public XrcException(string message) : base(message) { }
        public XrcException(string message, Exception inner) : base(message, inner) { }
        protected XrcException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
