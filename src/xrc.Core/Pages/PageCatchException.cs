using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Pages
{
    public class PageCatchException
    {
		public PageCatchException(string url)
        {
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException("url");

            Url = url;
        }

        public string Url
        {
            get;
            private set;
        }
    }
}
