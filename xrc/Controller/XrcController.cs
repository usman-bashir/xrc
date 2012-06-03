//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;

//namespace xrc
//{
//    public class XrcController : Controller
//    {
//        public void DoWork()
//        {
//            if (XrcApplication.Current == null)
//                throw new ApplicationException("XrcApplication not initialized.");

//            XrcApplication.Current.ProcessRequest(new XrcHttpRequest(Request),
//                                        new XrcHttpRequest(Response));
//        }
//    }
//}
