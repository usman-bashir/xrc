using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xrc
{
    public class StringResult : IResult
    {
        public StringResult()
        {
        }

        public StringResult(string content, Encoding contentEncoding = null, string contentType = null)
        {
            Content = content;
            ContentEncoding = contentEncoding;
            ContentType = contentType;
        }

        public string Content { get; set; }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public void Execute(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.Response;

            if (!String.IsNullOrEmpty(ContentType))
                response.ContentType = ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Content != null)
                response.Write(Content);
        }
    }
}
