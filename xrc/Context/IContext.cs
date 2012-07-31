using System;
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

        MashupPage Page { get; set; }

        ContextParameterList Parameters { get; }

        // TODO Non mi piace questo evento sul context...spostare forse sul kernel??
        RenderSlotEventHandler SlotCallback { get; set; }

        // TODO Anche questi due metodi qui non mi piacciono tanto...
		string GetAbsoluteUrl(string url);
		string GetAbsoluteFile(string file);
    }
}
