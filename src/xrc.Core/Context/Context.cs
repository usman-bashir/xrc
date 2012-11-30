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
		private XrcRequest _request;
		private XrcResponse _response;
        private ContextParameterList _parameters = new ContextParameterList();

		public Context(HttpContextBase httpContext)
		{
			_request = new XrcRequest(httpContext.Request);
			_response = new XrcResponse(httpContext.Response);
		}

		public Context(XrcRequest request, XrcResponse response)
        {
            _request = request;
            _response = response;
        }

		public Pages.IPage Page 
        { 
            get; 
            set; 
        }

		public XrcRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

		public XrcResponse Response
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

		public void CheckResponse()
        {
            if (Exception != null)
                throw Exception;

			// 
			//else if (Response.StatusCode < 200 || Response.StatusCode >= 300)
			//    throw new HttpException(Response.StatusCode, Response.StatusDescription ?? "Failed to process request.");
        }

		public IContext GetInitiatorContext()
		{
			return GetInitiatorContext(this);
		}

		private IContext GetInitiatorContext(IContext context)
		{
			if (context.CallerContext == null)
				return context;
			else
				return GetInitiatorContext(context.CallerContext);
		}
	}
}
