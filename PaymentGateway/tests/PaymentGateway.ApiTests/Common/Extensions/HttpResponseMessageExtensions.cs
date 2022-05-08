using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentGateway.Api.Json;
using Snapper;

namespace PaymentGateway.ApiTests.Common.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task ShouldMatchSnapshot(this HttpResponseMessage response, params string[] ignoredPaths)
    {
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        body.WithIgnores(ignoredPaths).ShouldMatchSnapshot();
    }

    public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
    {
        var payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        });
    }
}
