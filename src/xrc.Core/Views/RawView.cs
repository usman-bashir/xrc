using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using xrc.Pages.Providers;
using System.Web.Mvc;

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
					var result = new FilePathResult(ContentFile, contentType);
					if (!string.IsNullOrEmpty(FileDownloadName))
						result.FileDownloadName = FileDownloadName;

					ExecuteFileResult(context, result);
				}
				else
				{
					using (var contentStream = _resourceProvider.OpenResource(context.Page.GetResourceLocation(ContentFile)))
					{
						var result = new FileStreamResult(contentStream, contentType);
						if (!string.IsNullOrEmpty(FileDownloadName))
							result.FileDownloadName = FileDownloadName;

						ExecuteFileResult(context, result);
					}
				}
			}
			else if (Content != null)
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
			else
				throw new ArgumentNullException("Content");
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
