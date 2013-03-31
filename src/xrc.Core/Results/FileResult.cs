using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xrc
{
    // Code based from System.Web.Mvc.FileResult 
    // http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/7724375bcb597f43a184111a4f03711a3fa7d9d7#src/System.Web.Mvc/FileResult.cs

    public abstract class FileResult : IResult
    {
        protected FileResult(string contentType = null, string fileDownloadName = null)
        {
            ContentType = contentType;
            FileDownloadName = fileDownloadName;
        }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public string FileDownloadName { get; set; }

        public virtual void Execute(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.Response;

            if (!String.IsNullOrEmpty(ContentType))
                response.ContentType = ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (!String.IsNullOrEmpty(FileDownloadName))
            {
                // From RFC 2183, Sec. 2.3:
                // The sender may want to suggest a filename to be used if the entity is
                // detached and stored in a separate file. If the receiving MUA writes
                // the entity to a file, the suggested filename should be used as a
                // basis for the actual filename, where possible.
                string headerValue = ContentDispositionUtil.GetHeaderValue(FileDownloadName);
                response.AddHeader("Content-Disposition", headerValue);
            }

            WriteFile(response);
        }

        protected abstract void WriteFile(HttpResponseBase response);

        internal static class ContentDispositionUtil
        {
            private const string HexDigits = "0123456789ABCDEF";

            public static string GetHeaderValue(string fileName)
            {
                // If fileName contains any Unicode characters, encode according
                // to RFC 2231 (with clarifications from RFC 5987)
                if (fileName.Any(p => (int)p > 127))
                    return CreateRfc2231HeaderValue(fileName);

                // Knowing there are no Unicode characters in this fileName, rely on
                // ContentDisposition.ToString() to encode properly.
                // In .Net 4.0, ContentDisposition.ToString() throws FormatException if
                // the file name contains Unicode characters.
                // In .Net 4.5, ContentDisposition.ToString() no longer throws FormatException
                // if it contains Unicode, and it will not encode Unicode as we require here.
                // The Unicode test above is identical to the 4.0 FormatException test,
                // allowing this helper to give the same results in 4.0 and 4.5.         
                ContentDisposition disposition = new ContentDisposition() { FileName = fileName };
                return disposition.ToString();
            }

            private static string CreateRfc2231HeaderValue(string filename)
            {
                var builder = new StringBuilder("attachment; filename*=UTF-8''");

                byte[] filenameBytes = Encoding.UTF8.GetBytes(filename);
                foreach (byte b in filenameBytes)
                {
                    if (IsByteValidHeaderValueCharacter(b))
                    {
                        builder.Append((char)b);
                    }
                    else
                    {
                        AddByteToStringBuilder(b, builder);
                    }
                }

                return builder.ToString();
            }
            private static void AddByteToStringBuilder(byte b, StringBuilder builder)
            {
                builder.Append('%');

                int i = b;
                AddHexDigitToStringBuilder(i >> 4, builder);
                AddHexDigitToStringBuilder(i % 16, builder);
            }

            private static void AddHexDigitToStringBuilder(int digit, StringBuilder builder)
            {
                builder.Append(HexDigits[digit]);
            }

            // Application of RFC 2231 Encoding to Hypertext Transfer Protocol (HTTP) Header Fields, sec. 3.2
            // http://greenbytes.de/tech/webdav/draft-reschke-rfc2231-in-http-latest.html
            private static bool IsByteValidHeaderValueCharacter(byte b)
            {
                if ((byte)'0' <= b && b <= (byte)'9')
                {
                    return true; // is digit
                }
                if ((byte)'a' <= b && b <= (byte)'z')
                {
                    return true; // lowercase letter
                }
                if ((byte)'A' <= b && b <= (byte)'Z')
                {
                    return true; // uppercase letter
                }

                switch (b)
                {
                    case (byte)'-':
                    case (byte)'.':
                    case (byte)'_':
                    case (byte)'~':
                    case (byte)':':
                    case (byte)'!':
                    case (byte)'$':
                    case (byte)'&':
                    case (byte)'+':
                        return true;
                }

                return false;
            }
        }
    }
}
