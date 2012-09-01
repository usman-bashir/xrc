using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace xrc
{
    public static class ConvertEx
    {
        /// <summary>
        /// This method extends the Convert.ChangeType by supporting enum, nullable types and typeconverter.
        /// </summary>
        public static object ChangeType(object value, Type type, CultureInfo culture)
        {
            if (type.IsEnum)
            {
                int enumConditionValue;
                if (value is string)
                {
                    if (Int32.TryParse((string)value, out enumConditionValue))
                        return Enum.ToObject(type, enumConditionValue);
                    else
                        return Enum.Parse(type, (string)value, true);
                }
                else//Note: Conversion from Int32 to enum type is not automatically, so I first convert to an int then to enum using Enum.ToObject
                    return Enum.ToObject(type, Convert.ChangeType(value, typeof(int), culture));
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                    return null;
                else
                    return ChangeType(value, Nullable.GetUnderlyingType(type), culture);
            }
            else if (value == null)
                return value;
            else if (type.IsAssignableFrom(value.GetType()))
                return value;
            else if (value.GetType().IsEnum && type == typeof(string)) //Faccio un caso speciale per gli enum perchè altrimenti i Type Converter non gestiscono correttamente la conversione di valori di enum non validi
                return value.ToString();
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter != null && converter.CanConvertFrom(value.GetType()))
                    return converter.ConvertFrom(null, culture, value);
                else
                {
                    var converterTo = TypeDescriptor.GetConverter(value);
                    if (converterTo != null && converterTo.CanConvertTo(type))
                        return converterTo.ConvertTo(null, culture, value, type);
                    else
                        return Convert.ChangeType(value, type, culture);
                }
            }
        }
    }
}
