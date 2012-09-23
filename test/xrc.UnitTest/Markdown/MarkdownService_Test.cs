using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace xrc.Markdown
{
   
    [TestClass()]
	public class MarkdownService_Test
    {
		[TestMethod()]
		public void It_should_be_possible_to_get_all_configured_views()
		{
			var target = new MarkdownService();

			Assert.AreEqual("<p>ciao</p>\n", target.Transform("ciao"));
			Assert.AreEqual("<p>ciao <a href=\"http://www.google.com\">google</a></p>\n", target.Transform("ciao [google](http://www.google.com)"));
			Assert.AreEqual("<p>ciao <a href=\"/test.html\">test</a></p>\n", target.Transform("ciao [test](~/test.html)"));
		}
    }
}
