using System.Text.Json.Serialization.Metadata;

namespace WebApiSample.SystemTextJson
{
    public class JsonModifiers
    {
        /// <summary>
        /// Ensures that all value types not marked as nullable are required to be set explicitly
        /// in an incoming JSON payload.
        /// 
        /// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts
        /// </summary>
        /// <param name="typeInfo"></param>
        public static void MakeNonNullableValueTypesRequired(JsonTypeInfo typeInfo)
        {
            foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
            {
                var nullableType = typeof(Nullable<>);
                if (!propertyInfo.PropertyType.IsValueType || (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == nullableType))
                    continue;

                propertyInfo.IsRequired = true;
            }
        }
    }
}
