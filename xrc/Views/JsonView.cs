using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

namespace xrc.Views
{
    public class JsonView : IView
    {
		public JsonValue Content
		{
			get;
			set;
		}

		public void Execute(IContext context)
        {
			if (Content == null)
				throw new ArgumentNullException("Content");

            context.Response.ContentType = "application/json";
			context.Response.Output.Write(Content.ToString());
        }
    }
}
