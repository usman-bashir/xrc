using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using xrc.Razor;

namespace xrc.Razor
{
    /// <summary>
    /// Interceptor to invoke a module inside a page. This allow to create it using the factory and release it.
    /// Note: when MVC will correctly implement dependency injection for razor web page this class can be removed.
    /// </summary>
    class ModuleInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private ComponentDefinition _component;
        private IContext _context;
        private Modules.IModuleFactory _moduleFactory;
        public ModuleInterceptor(ComponentDefinition component, IContext context, Modules.IModuleFactory moduleFactory)
        {
            _component = component;
            _context = context;
            _moduleFactory = moduleFactory;
        }

        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            object module = _moduleFactory.Get(_component, _context);
            try
            {
                invocation.ReturnValue = invocation.Method.Invoke(module, invocation.Arguments);
            }
            finally
            {
                _moduleFactory.Release(module);
            }
        }
    }
}