using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Web.UI;

namespace xrc.Renderers
{
    public class XsltRenderer : IRenderer
    {
        private const string XSL_ARGUMENTS_NAMESPACE = "";
        private const string XSL_EXTENSIONS_PATTERN = "urn:xrc/{0}";
        private XslCompiledTransform _transform;
        private XsltSettings _settings = new XsltSettings();

        public XsltRenderer()
        {
            //this.Debug = false;
            this.Xslt = null;
        }

		//public bool Debug
		//{
		//    get;
		//    set;
		//}

		public XDocument Xslt
        {
            get;
            set;
        }
		//public bool EnableDocumentFunction 
		//{
		//    get { return _settings.EnableDocumentFunction;}
		//    set { _settings.EnableDocumentFunction = value; }
		//}
		//public bool EnableScript 
		//{
		//    get { return _settings.EnableScript; }
		//    set { _settings.EnableScript = value; }
		//}
        public XDocument Data
        {
            get;
            set;
        }

        public void RenderRequest(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            XDocument transformData = Data;
            if (transformData == null)
                transformData = new XDocument(); 

            if (Xslt == null)
                throw new ArgumentNullException("Xslt");

			// TODO Valutare se fare cache di questa classe
            _transform = new XslCompiledTransform(); //Debug);

			// TODO Valutare se CreateNavigator è sufficentemente performance e valutare anche il parameter XmlNameTable che potrebbe velocizzare la parsificazione...

            XmlResolver resolver = new XmlUrlResolver();
            _transform.Load(Xslt.CreateNavigator(), _settings, resolver);

            XsltArgumentList arguments = new XsltArgumentList();
            //arguments.XsltMessageEncountered += event

            foreach (var item in context.Parameters)
                arguments.AddParam(item.Key, XSL_ARGUMENTS_NAMESPACE, item.Value);

            foreach (var item in context.Modules)
                arguments.AddExtensionObject(string.Format(XSL_EXTENSIONS_PATTERN, item.Key.Name), item.Value);

			if (_transform.OutputSettings.OutputMethod == XmlOutputMethod.Html)
			{
				// TODO Valutare se effettivamente serve usare un writer particolare
				// da questo sembrerebbe di si: http://stackoverflow.com/questions/887739/xslt-self-closing-tags-issue
				// ma in realtà per risolvere il problema dei self enclosing io ho dovuto togliere il namespace 
				// xmlns="http://www.w3.org/1999/xhtml" da gli xslt figli, e lasciarlo solo sul padre...non ho capito perchè
				using (XhtmlTextWriter htmlOutput = new XhtmlTextWriter(context.Response.Output))
				{
					_transform.Transform(transformData.CreateNavigator(), arguments, htmlOutput);
				}
			}
			else
				_transform.Transform(transformData.CreateNavigator(), arguments, context.Response.Output);

            // TODO Should I need to set content type or other response properties?
			// maybe using some of the properties of the _transform.OutputSettings
        }
    }
}
