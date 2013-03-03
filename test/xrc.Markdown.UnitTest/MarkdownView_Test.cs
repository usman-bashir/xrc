using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using xrc.Markdown;
using xrc.Pages.Providers;
using Moq;

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
			var resourceProvider = new Mock<IResourceProviderService>();
			markdownService.Setup(p => p.Transform(content, null)).Returns("new text");

			var target = new MarkdownView(markdownService.Object, resourceProvider.Object);
			target.Content = content;

            string output;
            XrcRequest request = new XrcRequest(new XrcUrl("~/"));
            using (MemoryStream outStream = new MemoryStream())
            {
				XrcResponse response = new XrcResponse(outStream);
	            target.Execute(new Context(request, response));
				response.Flush();

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
