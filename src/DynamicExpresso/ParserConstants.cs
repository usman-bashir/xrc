using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DynamicExpresso
{
    public class ParserConstants
    {
        public static Type[] predefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };

        public static readonly Expression trueLiteral = Expression.Constant(true);
        public static readonly Expression falseLiteral = Expression.Constant(false);
        public static readonly Expression nullLiteral = Expression.Constant(null);

        public const string keywordIt = "it";
        public const string keywordIif = "iif";
        public const string keywordNew = "new";

        public const NumberStyles LiteralNumber = NumberStyles.AllowLeadingSign;
        public const NumberStyles LiteralUnsignedNumber = NumberStyles.None;
        public const NumberStyles LiteralDecimalNumber = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
    }
}
