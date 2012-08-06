using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    public class SlotModule : ISlotModule
    {
        private IContext _context;
        private IKernel _kernel;
        public SlotModule(IContext context, IKernel kernel)
        {
            _context = context;
            _kernel = kernel;
        }

        public string IncludeChild()
        {
            return IncludeChild(string.Empty);
        }

        public string IncludeChild(string slotName)
        {
            if (_context.SlotCallback != null)
            {
                RenderSlotEventArgs e = new RenderSlotEventArgs(slotName);
                _context.SlotCallback(this, e);
                return e.Content;
            }
            else
                return string.Empty;
        }


        private void Render(string url, Stream output, object parameters)
        {
            url = _context.GetAbsoluteUrl(url);
            XrcRequest request = new XrcRequest(new Uri(url));

            using (XrcResponse response = new XrcResponse(output))
            {
                Context context = new Context(request, response);
                AddParameters(context, parameters);

                _kernel.RenderRequest(context);

                context.CheckError();
            }
        }

        private void AddParameters(IContext context, object parameters)
        {
            if (parameters == null)
                return;

            // TODO Valutare se è possibile accettare i parametri in altro formato. Ma sembra che da xslt gli unici formati sono XPathNavigator o tipi primitivi (vedi http://msdn.microsoft.com/en-us/library/533texsx(VS.71).aspx ).
            // TODO Gestire anche altri tipi quando il metodo è chiamato da altri engine (es. razor)
            System.Xml.XPath.XPathNavigator parametersNode = parameters as System.Xml.XPath.XPathNavigator;
            if (parametersNode == null)
                throw new XrcException(string.Format("Parameters type '{0}' not supported.", parameters.GetType()));

            foreach (System.Xml.XPath.XPathNavigator nodeParameter in parametersNode.SelectChildren(XPathNodeType.Element))
            {
                context.Parameters.Add(new ContextParameter(nodeParameter.Name, typeof(string), nodeParameter.Value));
            }
        }

        public string Include(string url)
        {
            return Include(url, null);
        }

        public string Include(string url, object parameters)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Render(url, stream, parameters);

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
