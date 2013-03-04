using System.Web.Mvc;
using Castle.Windsor;
using System.Web;
using System.Collections.Generic;
using System;
using System.Reflection;
using Castle.MicroKernel.Registration;

namespace xrc.Web
{
	public class WindsorHttpModuleFactory : IHttpModule
	{
		static Castle.MicroKernel.IKernel _kernel;

		public static void Setup(Castle.MicroKernel.IKernel kernel)
		{
			_kernel = kernel;
		}

		IHttpModule[] _modules;

        public WindsorHttpModuleFactory()
		{
			if (_kernel == null)
				throw new InvalidOperationException("Container not initialized.");
		}

		public void Init(HttpApplication context)
		{
			_modules = GetRegisteredModules();

			InitModules(context);
		}

		public void Dispose()
		{
			ReleaseModules();
		}

		void ReleaseModules()
		{
			foreach (var module in _modules)
				_kernel.ReleaseComponent(module);
		}

		void InitModules(HttpApplication context)
		{
			foreach (var module in _modules)
				module.Init(context);
		}

		IHttpModule[] GetRegisteredModules()
		{
			return _kernel.ResolveAll<IHttpModule>();
		}
	}
}