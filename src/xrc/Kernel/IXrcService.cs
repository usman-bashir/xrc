﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace xrc
{
    public interface IXrcService
    {
		bool Match(IContext context);

		void ProcessRequest(IContext context);

        ContentResult Page(Uri url, object parameters = null, IContext callerContext = null);
    }
}