﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace xrc.Views
{
    // TODO This code is very similar to the one of HtmlModule.Slot. Maybe I can merge it?

    public class SlotView : IView
    {
        private IXrcService _xrcService;
        public SlotView(IXrcService xrcService)
        {
			_xrcService = xrcService;
        }

		public string SlotUrl
		{
			get;
			set;
		}

		public void Execute(IContext context)
        {
            if (SlotUrl == null)
                throw new ArgumentNullException("Slot");

			XrcUrl xrcUrl = context.Page.GetPageUrl(SlotUrl);

			ContentResult result = _xrcService.Page(xrcUrl, null, context);

			context.Response.ContentEncoding = result.ContentEncoding;
			context.Response.ContentType = result.ContentType;
			context.Response.Write(result.Content);
        }
    }
}
