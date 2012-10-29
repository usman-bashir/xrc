using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace xrc.Views
{
	public class RawView : IView
    {
        public byte[] Content
        {
            get;
            set;
        }

		public string ContentType
		{
			get;
			set;
		}

        public void Execute(IContext context)
        {
            if (Content == null)
                throw new ArgumentNullException("Content");

			if (ContentType != null)
				context.Response.ContentType = ContentType;

			// TODO Valutare se implementare dai metodi utilizzabili anche per content molto grandi (buffer?) 
			// o se leggere direttamente file senza passare da byte[]
            context.Response.OutputStream.Write(Content, 0, Content.Length);
        }
    }
}
