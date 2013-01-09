using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Pages.Providers;
using System.Web.Mvc;
using System.Web;
using System.Net;

namespace xrc.Views
{
	public class RawView : IView
    {
		readonly IResourceProviderService _resourceProvider;

		public RawView(IResourceProviderService resourceProvider)
		{
			_resourceProvider = resourceProvider;
		}

        public byte[] Content
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the file used for the response. Can be a resource (xrc style like ~/images/download.jpg) or a full path (c:\test\file.ext)
		/// </summary>
		public string ContentFile
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the ContentType. If not set the class try to discover the content type using the file extension of ContentFile.
		/// </summary>
		public string ContentType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the FileDownloadName. If set the header "Content-Disposition:attachment; filename=FileDownloadName" is used, otherwise "Content-Disposition:inline" is used.
		/// </summary>
		public string FileDownloadName
		{
			get;
			set;
		}

        public void Execute(IContext context)
        {
			if (!string.IsNullOrEmpty(ContentFile))
                ExecuteContentFile(context);
			else if (Content != null)
                ExecuteContent(context);
			else
				throw new ArgumentNullException("Content");
        }

        private void ExecuteContent(IContext context)
        {
            string contentType;
            if (ContentType != null)
                contentType = ContentType;
            else
                contentType = MimeTypes.UNKNOWN;

            var result = new FileContentResult(Content, contentType);
            if (!string.IsNullOrEmpty(FileDownloadName))
                result.FileDownloadName = FileDownloadName;

            ExecuteFileResult(context, result);
        }

        private void ExecuteContentFile(IContext context)
        {
            string contentType;
            if (ContentType != null)
                contentType = ContentType;
            else
            {
                string extension = System.IO.Path.GetExtension(ContentFile);
                contentType = MimeTypes.FromExtension(extension);
            }

            if (System.IO.Path.IsPathRooted(ContentFile))
            {
                if (!System.IO.File.Exists(ContentFile))
                    throw new HttpException((int)HttpStatusCode.NotFound, string.Format("Resource '{0}' not found.", ContentFile));

                var result = new FilePathResult(ContentFile, contentType);
                if (!string.IsNullOrEmpty(FileDownloadName))
                    result.FileDownloadName = FileDownloadName;

                ExecuteFileResult(context, result);
            }
            else
            {
                string resourceLocation = context.Page.GetResourceLocation(ContentFile);
                if (!_resourceProvider.ResourceExists(resourceLocation))
                    throw new HttpException((int)HttpStatusCode.NotFound, string.Format("Resource '{0}' not found.", resourceLocation));

                using (var contentStream = _resourceProvider.OpenResource(resourceLocation))
                {
                    var result = new FileStreamResult(contentStream, contentType);
                    if (!string.IsNullOrEmpty(FileDownloadName))
                        result.FileDownloadName = FileDownloadName;

                    ExecuteFileResult(context, result);
                }
            }
        }

		private static void ExecuteFileResult(IContext context, FileResult result)
		{
			ControllerContext controllerContext = new ControllerContext();
			controllerContext.HttpContext = new XrcHttpContext(context);
			controllerContext.RequestContext = new XrcRequestContext(context);
			result.ExecuteResult(controllerContext);
		}
    }
}
