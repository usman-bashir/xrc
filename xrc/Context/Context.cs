using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.SiteManager;
using xrc.Configuration;
using System.Web;

namespace xrc
{
    public class Context : IContext
    {
		private HttpRequestBase _request;
		private HttpResponseBase _response;
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private Dictionary<Module, object> _modules = new Dictionary<Module, object>();

		public Context(HttpRequestBase request, HttpResponseBase response)
        {
            _request = request;
            _response = response;
        }

        public ISiteConfiguration Configuration
        {
            get;
            set;
        }

        private MashupFile _file;
        public MashupFile File 
        {
            get { return _file; }
            set
            {
                _file = value;
                if (_file != null)
                    WorkingPath = _file.WorkingPath;
            }
        }
        public string WorkingPath
        {
            get;
            set;
        }

        public MashupPage Page 
        { 
            get; 
            set; 
        }

		public HttpRequestBase Request
        {
            get { return _request; }
            set { _request = value; }
        }

		public HttpResponseBase Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        public Dictionary<Module, object> Modules
        {
            get { return _modules; }
        }

        public RenderSlotEventHandler SlotCallback
        {
            get;
            set;
        }
    }
}
