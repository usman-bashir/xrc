using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Sites;
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

		public Pages.IPage Page 
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

        public Exception Exception 
        { 
            get; 
            set; 
        }

		public IContext CallerContext { get; set; }

        public void CheckError()
        {
            if (Exception != null)
                throw Exception;
            else if (Response.StatusCode < 200 || Response.StatusCode >= 300)
                throw new HttpException(Response.StatusCode, Response.StatusDescription ?? "Failed to process request.");
        }
    }
}
