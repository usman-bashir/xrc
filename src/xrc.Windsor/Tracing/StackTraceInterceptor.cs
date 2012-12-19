using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using System.Diagnostics;
using System.Threading;

namespace xrc.Windsor.Tracing
{
	public class StackTraceInterceptor : IInterceptor
	{
		readonly TraceSource TraceSource = new TraceSource("StackTrace", SourceLevels.All);
		int _methodCountId = 0;

		public void Intercept(IInvocation invocation)
		{
			// TODO Valutare se aggiungere questo if per performance
			//if (TraceSource.Switch.ShouldTrace(TraceEventType.Information))

			int mcount = Interlocked.Increment(ref _methodCountId);

			TraceSource.TraceEvent(TraceEventType.Information, mcount, CreateInvocationLogString(invocation));
			try
			{
				invocation.Proceed();
			}
			catch (Exception ex)
			{
				TraceSource.TraceEvent(TraceEventType.Warning, mcount, string.Format("Exception on {0} {1}", CreateInvocationLogString(invocation), ex));
				throw;
			}
		}

		public static String CreateInvocationLogString(IInvocation invocation)
		{
			var sb = new StringBuilder(100);
			sb.AppendFormat("{0}.{1}(", invocation.TargetType.Name, invocation.Method.Name);
			foreach (object argument in invocation.Arguments)
			{
				var argumentDescription = argument == null ? "null" : DumpObject(argument);
				sb.Append(argumentDescription).Append(",");
			}
			if (invocation.Arguments.Count() > 0) 
				sb.Length--;

			sb.Append(")");

			return sb.ToString();
		}

		static string DumpObject(object argument)
		{
			if (argument == null)
				return "null";
			else
				return string.Format("{0}[{1}]", argument.GetType().Name, argument.ToString());

			//Type objtype = argument.GetType();
			//if (objtype == typeof(String) || objtype.IsPrimitive || !objtype.IsClass)
			//    return objtype.ToString();

			//return DataContractSerialize(argument, objtype);
		}

		//static string EscapeTraceString(string value)
		//{
		//    return value.Replace("\t", @"\t")
		//                .Replace("\r", @"\r")
		//                .Replace("\n", @"\n");
		//}
	}
}
