using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentGateway.Api.Json;

namespace PaymentGateway.ApiTests.Common.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> CreateAsync<T>(this HttpClient client, string url, T body)
        => await client.PostAsJsonAsync(url, body, new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        });
}