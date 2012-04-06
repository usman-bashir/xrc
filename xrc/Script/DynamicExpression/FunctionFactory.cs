using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;

// See http://www.fidelitydesign.net/?p=333

namespace DynamicExpression
{
    internal static class FunctionFactory
    {
		public static Delegate Create(Type returnType, string expression, ParameterExpression[] arguments)
		{
			ExpressionParser parser = new ExpressionParser(arguments, expression, null);

			var @delegate = Expression.Lambda(parser.Parse(returnType), arguments).Compile();
			return @delegate;
		}

        public static Delegate Create(Type[] argumentTypes, Type returnType, string expression, string[] argumentNames = null)
        {
            if (argumentNames != null)
            {
                if (argumentTypes.Length != argumentNames.Length)
                    throw new ArgumentException("The number of argument names does not equal the number of argument types.");
            }

            var @params = argumentTypes
                .Select((t, i) => (argumentNames == null)
                                      ? Expression.Parameter(t)
                                      : Expression.Parameter(t, argumentNames[i]))
                .ToArray();

			return Create(returnType, expression, @params);
        }

        public static Func<TReturn> Create<TReturn>(string expression, string[] argumentNames = null)
        {
            return (Func<TReturn>)Create(new Type[0], typeof(TReturn), expression, argumentNames);
        }

        public static Func<A, TReturn> Create<A, TReturn>(string expression, string[] argumentNames = null)
        {
            return (Func<A, TReturn>)Create(new[] { typeof(A) }, typeof(TReturn), expression, argumentNames);
        }

        public static Func<A, B, TReturn> Create<A, B, TReturn>(string expression, string[] argumentNames = null)
        {
            return (Func<A, B, TReturn>)Create(new[] { typeof(A), typeof(B) }, typeof(TReturn), expression, argumentNames);
        }
    }
}
