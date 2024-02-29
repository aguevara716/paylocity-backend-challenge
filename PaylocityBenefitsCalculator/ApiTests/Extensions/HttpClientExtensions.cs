using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTests.Extensions;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, string url, T requestBody)
    {
        var requestBodyJson = JsonSerializer.Serialize(requestBody);
        var stringContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
        return httpClient.PostAsync(url, stringContent);
    }
}
