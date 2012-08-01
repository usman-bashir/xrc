using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Net;

namespace xrc.Modules
{
    public class HtmlModule : IModule
    {
        private IContext _context;
        private IKernel _kernel;
        public HtmlModule(IContext context, IKernel kernel)
        {
            _context = context;
            _kernel = kernel;
        }

        public void RenderAction(string url, Stream output)
        {
			url = _context.GetAbsoluteUrl(url);
            XrcRequest request = new XrcRequest(new Uri(url));

            using (XrcResponse response = new XrcResponse(output))
            {
                Context context = new Context(request, response);
                _kernel.RenderRequest(context);

                context.CheckError();
			}
        }

        public string Action(string url)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                RenderAction(url, stream);

                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
