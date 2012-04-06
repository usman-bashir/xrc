using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace xrc.Reflection
{
    public static class Reflection
    {
        const BindingFlags PROPERTY_BINDINGFLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default;

        public static PropertyInfo GetPropertyInfo(Type type, string property)
        {
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentException("Property cannot be empty");

            var propertyInfo = type.GetProperty(property, PROPERTY_BINDINGFLAGS);
            if (propertyInfo == null)
            {
                if (type.BaseType == typeof(Object))
                    throw new ArgumentException(string.Format("Property {0} not found on type {1}", property, type));
                return GetPropertyInfo(type.BaseType, property);
            }

            return propertyInfo;
        }

        public static Type GetPropertyType(this object obj, string property)
        {
            return GetPropertyInfo(obj.GetType(), property).PropertyType;
        }

        public static object GetProperty(this object obj, string property)
        {
            return GetPropertyInfo(obj.GetType(), property).GetValue(obj, null);
        }

        public static void SetProperty(this object obj, string property, object value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentException("Property cannot be empty");

            var propertyInfo = GetPropertyInfo(obj.GetType(), property);
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Property {0} not found on type {1}", property, obj.GetType()));

            propertyInfo.SetValue(obj, value, null);
        }

        public static Type GetExpressionType(this object obj, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be empty");

            string[] parts = expression.Split(new char[] { '.' }, 2);
            if (parts.Length == 2)
            {
                object nestedObj = GetProperty(obj, parts[0]);
                if (nestedObj == null)
                    return null;
                else
                    return GetExpressionType(nestedObj, parts[1]);
            }
            else if (parts.Length == 1)
            {
                return GetPropertyType(obj, parts[0]);
            }
            else
                throw new ApplicationException(string.Format("Invalid expression {0}", expression));
        }

        public static object GetExpressionValue(this object obj, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be empty");

            string[] parts = expression.Split(new char[] { '.' }, 2);
            if (parts.Length == 2)
            {
                object nestedObj = GetProperty(obj, parts[0]);
                if (nestedObj == null)
                    return null;
                else
                    return GetExpressionValue(nestedObj, parts[1]);
            }
            else if (parts.Length == 1)
            {
                return GetProperty(obj, parts[0]);
            }
            else
                throw new ApplicationException(string.Format("Invalid expression {0}", expression));
        }

        public static void SetExpressionValue(this object obj, string expression, object value)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be empty");

            string[] parts = expression.Split(new char[] { '.' }, 2);
            if (parts.Length == 2)
            {
                object nestedObj = GetProperty(obj, parts[0]);
                if (nestedObj == null)
                {
                    //throw new NullReferenceException(string.Format("Expression {0} contains a null reference:{1}", expression, parts[0]));

                    //If nested is null try to create the nested object
                    PropertyInfo propInfo = GetPropertyInfo(obj.GetType(), parts[0]);
                    nestedObj = Activator.CreateInstance(propInfo.PropertyType);
                    SetProperty(obj, parts[0], nestedObj);
                }

                SetExpressionValue(nestedObj, parts[1], value);
            }
            else if (parts.Length == 1)
            {
                SetProperty(obj, parts[0], value);
            }
            else
                throw new ApplicationException(string.Format("Invalid expression {0}", expression));
        }

        public static object GetDefaultOfType(Type conditionType)
        {
            if (conditionType.IsValueType)
                return Activator.CreateInstance(conditionType);
            else
                return null;
        }

        public static string ToGenericTypeString(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            string genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            string genericArgs = string.Join(",", t.GetGenericArguments().Select(ToGenericTypeString));
            return genericTypeName + "<" + genericArgs + ">";
        }

        public static string GetDisplayName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            string genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            return genericTypeName;
        }

        public static bool HasDefaultConstructor(this Type t)
        {
            return t.GetConstructors().Where(c => c.GetParameters().Length <= 0).Count() > 0;
        }

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
