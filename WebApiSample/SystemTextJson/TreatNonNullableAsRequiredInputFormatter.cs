using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApiSample.SystemTextJson
{
    /// <summary>
    /// TODO: Try to "mark" non-nullable value types as "required" (same logic as the C# keyword)...
    /// </summary>
    public class TreatNonNullableAsRequiredInputFormatter : InputFormatter
    {
        public TreatNonNullableAsRequiredInputFormatter()
        {
            SupportedMediaTypes.Add("application/json");
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var bla = context.Metadata.ContainerType;

            return InputFormatterResult.Success(null);
            //using (var reader = new StreamReader(context.HttpContext.Request.Body))
            //{
            //    string csv = await reader.ReadToEndAsync();
            //    var values = csv.Split(",");
            //    return InputFormatterResult.Success(values);
            //}
        }
        protected override bool CanReadType(Type type)
        {
            //Which action parameter types this InputFormatter can handle
            return true;
        }
    }
}
