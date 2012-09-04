﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace $rootnamespace$
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
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