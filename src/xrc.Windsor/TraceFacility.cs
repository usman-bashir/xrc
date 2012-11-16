using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using Castle.Core;

namespace xrc.IoC.Windsor
{
	public class TraceFacility : AbstractFacility
	{
		protected override void Init()
		{
			Kernel.ComponentRegistered += new ComponentDataDelegate(Kernel_ComponentRegistered);
		}

		void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
		{
			handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(TraceCallInterceptor)));
		}
	}
}
