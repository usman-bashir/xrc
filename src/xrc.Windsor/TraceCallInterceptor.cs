using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace xrc.IoC.Windsor
{
	public class TraceCallInterceptor : IInterceptor
	{
		//public void Intercept(IInvocation invocation)
		//{
		//    if (Logger.IsDebugEnabled) Logger.Debug(CreateInvocationLogString(invocation));
		//    try
		//    {
		//        invocation.Proceed();
		//    }
		//    catch (Exception ex)
		//    {
		//        if (Logger.IsErrorEnabled) Logger.Error(CreateInvocationLogString(invocation), ex);
		//        throw;
		//    }
		//}

		public void Intercept(IInvocation invocation)
		{
			System.Diagnostics.Trace.WriteLine(CreateInvocationLogString(invocation));
			try
			{
				invocation.Proceed();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(string.Format("ERROR {0} {1}", CreateInvocationLogString(invocation), ex));
				throw;
			}
		}

		public static String CreateInvocationLogString(IInvocation invocation)
		{
			StringBuilder sb = new StringBuilder(100);
			sb.AppendFormat("Called: {0}.{1}(", invocation.TargetType.Name, invocation.Method.Name);
			foreach (object argument in invocation.Arguments)
			{
				String argumentDescription = argument == null ? "null" : DumpObject(argument);
				sb.Append(argumentDescription).Append(",");
			}
			if (invocation.Arguments.Count() > 0) sb.Length--;
			sb.Append(")");
			return sb.ToString();
		}

		private static string DumpObject(object argument)
		{
			if (argument == null)
				return "<null>";
			else
				return argument.ToString();

			//Type objtype = argument.GetType();
			//if (objtype == typeof(String) || objtype.IsPrimitive || !objtype.IsClass)
			//    return objtype.ToString();

			//return DataContractSerialize(argument, objtype);
		}
	}
}
