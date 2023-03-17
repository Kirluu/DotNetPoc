using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns true for int, long, double, decimal, etc., but returns false for int?, long?, double?, decimal?, etc.
        /// Also returns false for anything not a value type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonNullableValueType(this Type type)
        {
            return type.IsValueType && !type.IsNullable();
        }

        /// <summary>
        /// Returns true for int[], MyObject[], List&lt;MyObject&gt;, etc., but returns false for int[]?, MyObject[]?, List&lt;MyObject&gt;?, etc.
        /// Also returns false for anything not implementing IEnumerable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonNullableEnumerable(this Type type)
        {
            if (type.IsNullable()) return false;
            if (type.IsValueType) return false;

            return type.IsEnumerable();
        }

        /// <summary>
        /// Returns true for MyObject, , etc., but returns false for int[]?, MyObject[]?, List&lt;MyObject&gt;?, etc.
        /// Also returns false for anything not implementing IEnumerable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNonNullableReferenceType(this Type type)
        {
            if (type.IsNullable()) return false;
            if (type.IsValueType) return false;
            if (type.IsEnumerable()) return false;
            if (type == typeof(string)) return false;

            // TODO: More checks?

            return true;
        }

        public static bool IsNullable(this Type type)
        {
            var nullableType = typeof(Nullable<>);
            return type.IsGenericType && type.GetGenericTypeDefinition() == nullableType;
        }

        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static Type GetEnumerableElementType(this Type type)
        {
            if (!type.IsEnumerable())
                throw new ArgumentException($"The given type '{type}' is not an enumerable.");

            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                if (genericArguments.Length == 1)
                    return type.GetGenericArguments().First();

                throw new ArgumentException($"Failed to extract inner type of Enumerable type '{type}', because it has more than one generic argument.");
            }

            throw new ArgumentException($"Failed to extract inner type of Enumerable type '{type}' because it is not an array, nor generic.");
        }
    }
}
