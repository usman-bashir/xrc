using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace xrc.Renderers
{
    // TODO This code is very similar to the one of HtmlModule.Slot. Maybe I can merge it?

    public class SlotRenderer : IRenderer
    {
        private IKernel _kernel;
        public SlotRenderer(IKernel kernel)
        {
            _kernel = kernel;
        }

		public string SlotUrl
		{
			get;
			set;
		}

		public void RenderRequest(IContext context)
        {
            if (SlotUrl == null)
                throw new ArgumentNullException("Slot");

			var url = context.GetAbsoluteUrl(SlotUrl);
            XrcRequest request = new XrcRequest(new Uri(url));

            Context slotContext = new Context(request, context.Response);
            _kernel.RenderRequest(slotContext);

            slotContext.CheckError();
        }
    }
}
