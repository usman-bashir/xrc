using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace xrc.Web
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
		private readonly Castle.MicroKernel.IKernel kernel;

        public WindsorControllerFactory(Castle.MicroKernel.IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
			if (controllerType != null)
				return (IController)kernel.Resolve(controllerType);
			else
				return base.GetControllerInstance(requestContext, controllerType);

			//if (controllerType == null)
			//{
			//    throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
			//}
			//return (IController)kernel.Resolve(controllerType);
        }
    }
}