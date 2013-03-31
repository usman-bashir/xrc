using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xrc
{
    public class StreamResult : FileResult
    {
        // default buffer size as defined in BufferedStream type
        private const int BufferSize = 0x1000;

        public StreamResult(Stream stream, string contentType, string fileDownloadName = null)
            : base(contentType, fileDownloadName)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            ResultStream = stream;
        }

        public Stream ResultStream { get; private set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            // grab chunks of data and write to the output stream
            Stream outputStream = response.OutputStream;
            using (ResultStream)
            {
                byte[] buffer = new byte[BufferSize];

                while (true)
                {
                    int bytesRead = ResultStream.Read(buffer, 0, BufferSize);
                    if (bytesRead == 0)
                    {
                        // no more data
                        break;
                    }

                    outputStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}
