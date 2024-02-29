using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTests.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T?> DeserializeTo<T>(this HttpContent content)
    {
        var contentJson = await content.ReadAsStringAsync();
        var contentObject = JsonSerializer.Deserialize<T>(contentJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return contentObject;
    }
}
