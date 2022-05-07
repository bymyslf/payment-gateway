using PaymentGateway.Core.UseCases.PaymentRequest;
using static PaymentGateway.UnitTests.Builders.PaymentRequestCardBuilder;

namespace PaymentGateway.UnitTests.Builders;

internal class PaymentRequestBuilder
{
    private const int DefaultAmount = 1000;
    private const string DefaultCurrency = "EUR";
    
    public static PaymentRequestBuilder PaymentRequest()
        => new();

    private int _amount;
    private string _currency;
    private PaymentRequestCard _card;
    
    private PaymentRequestBuilder()
    { }
    
    public PaymentRequestBuilder TestValues()
    {
        Amount(DefaultAmount);
        Currency(DefaultCurrency);
        Card(PaymentRequestCard().TestValues().Build());
        return this;
    } 

    public PaymentRequestBuilder Amount(int amount)
    {
        _amount = amount;
        return this;
    }
    
    public PaymentRequestBuilder Currency(string currency)
    {
        _currency = currency;
        return this;
    }
    
    public PaymentRequestBuilder Card(PaymentRequestCard card)
    {
        _card = card;
        return this;
    }
    
    public PaymentRequest Build()
        => new(_amount, _currency, _card);
}