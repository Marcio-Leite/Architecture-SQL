using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Services.Tests
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonSerializer.Serialize(obj), Encoding.Default, "application/json");
    }
}