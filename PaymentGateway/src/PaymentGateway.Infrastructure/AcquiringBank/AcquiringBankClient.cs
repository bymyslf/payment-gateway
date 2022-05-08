using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.Infrastructure.AcquiringBank;

public class AcquiringBankClient : IAcquiringBank
{
    private static readonly JsonSerializerSettings SerializerSettings = new() { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } };
    private static readonly JsonSerializer Serializer = JsonSerializer.Create(SerializerSettings);
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<AcquiringBankClient> _logger;
    
    public AcquiringBankClient(HttpClient httpClient, ILogger<AcquiringBankClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<PayoutResult> ProcessPayout(Payment payment, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object> { ["PaymentId"] =  payment.Id });
        
        try
        {
            using var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri("payments", UriKind.Relative) };
            httpRequestMessage.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/json");
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                payment.Amount,
                CardName = payment.Card.HolderName,
                CardNumber = payment.Card.Number,
                CardExpireMonth = payment.Card.ExpireMonth,
                CardExpireYear = payment.Card.ExpireYear,
                CardCVV = payment.Card.Cvv
            }), Encoding.UTF8, "application/json");
            
            _logger.LogInformation("Send payment to AcquiringBank");
            
            using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);
                using var sr = new StreamReader(responseStream);
                using var jsonTextReader = new JsonTextReader(sr);
                var payoutResponse = Serializer.Deserialize<PayoutResponse>(jsonTextReader);
                
                _logger.LogInformation("AcquiringBank payment succeeded");
                
                return new PayoutResult
                {
                    Succeeded = true,
                    PayoutId = payoutResponse.PayoutId
                };
            }

            _logger.LogWarning("AcquiringBank payment failed");
            
            return new PayoutResult
            {
                Message = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken)
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error communicating with AcquiringBank");
            return new PayoutResult
            {
                Message = $"AcquiringBank communication error: '{e.Message}'"
            };
        }
    }

    private record PayoutResponse(string PayoutId);
}