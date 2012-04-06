using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    // TODO This code is very similar to the one of HtmlModule.Slot. Maybe I can merge it?

    public class HtmlModule : IHtmlModule
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
            var cfg = _context.Configuration;
            url = cfg.UrlContent(url, _context.Request.Url);
            XrcRequest request = new XrcRequest(new Uri(url));

            using (XrcResponse response = new XrcResponse(output))
            {
                Context context = new Context(request, response);
                _kernel.RenderRequest(context);

				// TODO Here I must check the response if it is valid, otherwise throw an exception?
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
