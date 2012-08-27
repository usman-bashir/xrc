using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System.Xml.XPath;

namespace xrc
{
    public class XrcService : IXrcService
    {
        private IKernel _kernel;

        public XrcService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public ContentResult Page(Uri url, object parameters = null, IContext callerContext = null)
        {
            ContentResult contentResult = new ContentResult();

			var parentRequest = callerContext == null ? null : callerContext.Request;
			var parentResponse = callerContext == null ? null : callerContext.Response;

            using (MemoryStream stream = new MemoryStream())
            {
                XrcRequest request = new XrcRequest(url, parentRequest: parentRequest);

                using (XrcResponse response = new XrcResponse(stream, parentResponse: parentResponse))
                {
                    Context context = new Context(request, response);
					context.CallerContext = callerContext;
                    AddParameters(context, parameters);

                    _kernel.ProcessRequest(context);

                    context.CheckError(); // TODO Bisogna gestire anche il redirect e quindi richiamare la pagina in redirect?

                    contentResult.ContentEncoding = response.ContentEncoding;
                    contentResult.ContentType = response.ContentType;
                }

                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    contentResult.Content = reader.ReadToEnd();
                }
            }

            return contentResult;
        }

        private void AddParameters(IContext context, object parameters)
        {
            if (parameters == null)
                return;

            // TODO Valutare se è possibile accettare i parametri in altro formato. Ma sembra che da xslt gli unici formati sono XPathNavigator o tipi primitivi (vedi http://msdn.microsoft.com/en-us/library/533texsx(VS.71).aspx ).
            // TODO Gestire anche altri tipi quando il metodo è chiamato da altri engine (es. razor)
            if (parameters is System.Xml.XPath.XPathNavigator)
            {
                System.Xml.XPath.XPathNavigator parametersNode = parameters as System.Xml.XPath.XPathNavigator;
                foreach (System.Xml.XPath.XPathNavigator nodeParameter in parametersNode.SelectChildren(XPathNodeType.Element))
                {
                    context.Parameters.Add(new ContextParameter(nodeParameter.Name, typeof(string), nodeParameter.Value));
                }
            }
            else
            {
                // Read all the properties (used when parameters is for example an anonymous type)
                foreach (var p in parameters.GetType().GetProperties())
                {
                    context.Parameters.Add(new ContextParameter(p.Name, p.PropertyType, p.GetValue(parameters, null)));
                }
            }
            //else
            //    throw new XrcException(string.Format("Parameters type '{0}' not supported.", parameters.GetType()));
        }

    }
}
