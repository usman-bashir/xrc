using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using Moq;
using xrc.Markdown;

namespace xrc.Views
{
    [TestClass]
	public class MarkdownView_Test
    {
        [TestMethod]
        public void Transform_Simple()
        {
			string content = "ciao";
			var markdownService = new Mock<IMarkdownService>();
			markdownService.Setup(p => p.Transform(content, null)).Returns("new text");

			var target = new MarkdownView(markdownService.Object);
			target.Content = content;

            string output;
            XrcRequest request = new XrcRequest(new Uri("http://test/"));
            using (MemoryStream outStream = new MemoryStream())
            {
                using (XrcResponse response = new XrcResponse(outStream))
	                target.Execute(new Context(request, response));

                outStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(outStream))
                {
                    output = reader.ReadToEnd();
                }
            }

			Assert.AreEqual("new text", output);
        }
    }
}
