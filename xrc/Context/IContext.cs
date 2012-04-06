﻿using System;
using System.Collections.Generic;
using xrc.SiteManager;
using xrc.Configuration;
using System.Web;

namespace xrc
{
    public interface IContext
    {
        HttpRequestBase Request { get; set; }
        HttpResponseBase Response { get; set; }

        ISiteConfiguration Configuration { get; set; }

        string WorkingPath { get; set; }
        MashupFile File { get; set; }

        Dictionary<string, string> Parameters { get; }

        MashupPage Page { get; set; }

        Dictionary<Module, object> Modules { get; }

        RenderSlotEventHandler SlotCallback { get; set; }
    }
}
