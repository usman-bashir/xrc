using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.Registration;

namespace xrc.Windsor.Tracing
{
	public class TraceFacility : AbstractFacility
	{
		protected override void Init()
		{
			Kernel.Register(Component.For<StackTraceInterceptor>());

			Kernel.ComponentRegistered += new ComponentDataDelegate(Kernel_ComponentRegistered);
		}

		void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
		{
			handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(StackTraceInterceptor)));
		}
	}
}
