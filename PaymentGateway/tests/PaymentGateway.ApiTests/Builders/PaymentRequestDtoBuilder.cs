using PaymentGateway.Api.Features;
using PaymentGateway.Core.Domain;
using static PaymentGateway.ApiTests.Builders.PaymentRequestCardDtoBuilder;

namespace PaymentGateway.ApiTests.Builders;

internal class PaymentRequestDtoBuilder
{
    private const int DefaultAmount = 1000;
    private const string DefaultCurrency = "EUR";
    
    public static PaymentRequestDtoBuilder PaymentRequestDto()
        => new();

    private int _amount;
    private string _currency;
    private PaymentRequestCardDto _card;
    
    private PaymentRequestDtoBuilder()
    { }
    
    public PaymentRequestDtoBuilder TestValues()
    {
        Amount(DefaultAmount);
        Currency(DefaultCurrency);
        Card(PaymentRequestCardDto().TestValues().Build());
        return this;
    } 

    public PaymentRequestDtoBuilder Amount(int amount)
    {
        _amount = amount;
        return this;
    }
    
    public PaymentRequestDtoBuilder Currency(string currency)
    {
        _currency = currency;
        return this;
    }
    
    public PaymentRequestDtoBuilder Card(PaymentRequestCardDto card)
    {
        _card = card;
        return this;
    }
    
    public PaymentRequestDto Build()
        => new(_amount, _currency, _card);
}