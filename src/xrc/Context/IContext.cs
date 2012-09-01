using System;
using System.Collections.Generic;
using System.Web;
using xrc.Sites;

namespace xrc
{
	// TODO Rivedere la classe context...forse eliminare
    public interface IContext
    {
        HttpRequestBase Request { get; set; }
        HttpResponseBase Response { get; set; }

        Pages.IPage Page { get; set; }

        ContextParameterList Parameters { get; }

        Exception Exception { get; set; }

        // TODO Non mi piace questo evento sul context...da spostare...
        RenderSlotEventHandler SlotCallback { get; set; }

		IContext CallerContext { get; set; }

		void CheckResponse();
    }
}
