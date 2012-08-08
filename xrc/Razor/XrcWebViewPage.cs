using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using xrc.Razor;

namespace xrc.Razor
{
    // A custom razor page that can be used as a base class for razor page.
    //  The problem is that currently I cannot find an easy method to IoC with WebViewPage so I use a dynamic proxy that create the modules and release it.
    //  I load all the modules and parameters defined in the page (using the @functions keyword).
    public abstract class XrcWebViewPage<TModel> : WebViewPage<TModel>
    {
        public XrcWebViewPage()
        {
        }

        XrcWebViewPageExtension _xrcExtension;
        public override void InitHelpers()
        {
            base.InitHelpers();

            _xrcExtension = new XrcWebViewPageExtension(this);
        }
    }

    public abstract class XrcWebViewPage : WebViewPage
    {
        public XrcWebViewPage()
        {
        }

        XrcWebViewPageExtension _xrcExtension;
        public override void InitHelpers()
        {
            base.InitHelpers();

            _xrcExtension = new XrcWebViewPageExtension(this);
        }
    }
}