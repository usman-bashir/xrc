using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xrc
{
    public class BytesResult : FileResult
    {
        public BytesResult(byte[] content, string contentType, string fileDownloadName = null)
            : base(contentType, fileDownloadName)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            Content = content;
        }

        public byte[] Content { get; private set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.OutputStream.Write(Content, 0, Content.Length);
        }
    }
}
