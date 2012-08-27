using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages;

namespace xrc.Mocks
{
    public class ContextMock : IContext
    {
        public System.Web.HttpRequestBase Request
        {
            get;
            set;
        }

        public System.Web.HttpResponseBase Response
        {
            get;
            set;
        }

        private ContextParameterList _parameters = new ContextParameterList();
        public ContextParameterList Parameters
        {
            get { return _parameters; }
        }

        public IPage Page
        {
            get;
            set;
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

        public void CheckError()
        {
            
        }


		public IContext CallerContext
		{
			get;
			set;
		}
	}
}
