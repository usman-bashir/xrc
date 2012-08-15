using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace xrc.Views
{
    // TODO This code is very similar to the one of HtmlModule.Slot. Maybe I can merge it?

    public class SlotView : IView
    {
        private IKernel _kernel;
        public SlotView(IKernel kernel)
        {
            _kernel = kernel;
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

			var url = context.GetAbsoluteUrl(SlotUrl);
            XrcRequest request = new XrcRequest(new Uri(url), parentRequest: context.Request);

            Context slotContext = new Context(request, context.Response);
            _kernel.ProcessRequest(slotContext);

            slotContext.CheckError();
        }
    }
}
