using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc
{
    [Serializable]
    public class DuplicateItemException : XrcException
    {
        public DuplicateItemException(string key)
			: base(string.Format("An item with the same key, '{0}', has already been added.", key)) 
        {
			Key = key;
        }

		public string Key
        {
            get;
            private set;
        }

		protected DuplicateItemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
