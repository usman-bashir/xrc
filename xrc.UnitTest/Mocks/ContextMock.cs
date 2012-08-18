using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Sites.ISiteConfiguration Configuration
        {
            get;
            set;
        }

        public string WorkingPath
        {
            get
            {
                return "testValue";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public SiteManager.MashupFile File
        {
            get;
            set;
        }

        private ContextParameterList _parameters = new ContextParameterList();
        public ContextParameterList Parameters
        {
            get { return _parameters; }
        }

        public SiteManager.MashupPage Page
        {
            get;
            set;
        }

        public RenderSlotEventHandler SlotCallback
        {
            get;
            set;
        }

        public string GetAbsoluteUrl(string url)
        {
            return url;
        }

        public string GetAbsoluteFile(string file)
        {
            return file;
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
