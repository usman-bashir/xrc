using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xrc
{
    public class FilePathResult : FileResult
    {
        public FilePathResult(string fileName, string contentType, string fileDownloadName = null)
            : base(contentType, fileDownloadName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            FileName = fileName;
        }

        public string FileName { get; private set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.TransmitFile(FileName);
        }
    }
}
