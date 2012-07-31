using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.SiteManager;
using xrc.Configuration;
using System.Web;
using System.IO;

namespace xrc
{
    public class Context : IContext
    {
		private HttpRequestBase _request;
		private HttpResponseBase _response;
        private ContextParameterList _parameters = new ContextParameterList();

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

        public ContextParameterList Parameters
        {
            get { return _parameters; }
        }

        public RenderSlotEventHandler SlotCallback
        {
            get;
            set;
        }

		public string GetAbsoluteUrl(string url)
		{
			return Configuration.GetAbsoluteUrl(url, Request.Url);
		}

		public string GetAbsoluteFile(string file)
		{
			if (VirtualPathUtility.IsAppRelative(file))
				return VirtualPathUtility.ToAbsolute(file);
			else
				return Path.Combine(WorkingPath, file);
		}
    }
}
