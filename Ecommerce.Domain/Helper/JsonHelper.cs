using Newtonsoft.Json;

namespace Ecommerce.Domain.Helper
{
    public static class JsonHelper
    {
        public static string ToJson(object input, string methodName, string message)
        {
            return JsonConvert.SerializeObject(new { message, method = methodName, input }, Formatting.Indented);
        }
    }
}
