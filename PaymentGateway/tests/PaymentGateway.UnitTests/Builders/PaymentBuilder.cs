using PaymentGateway.Core.Domain;
using static PaymentGateway.UnitTests.Builders.PaymentCardBuilder;

namespace PaymentGateway.UnitTests.Builders;

internal class PaymentBuilder
{
    private const int DefaultAmount = 1000;
    private const string DefaultCurrency = "EUR";
    
    public static PaymentBuilder Payment()
        => new();

    private int _amount;
    private string _currency;
    private PaymentCard _card;
    
    private PaymentBuilder()
    { }

    public PaymentBuilder TestValues()
    {
        Amount(DefaultAmount);
        Currency(DefaultCurrency);
        Card(PaymentCard().TestValues().Build());
        return this;
    }

    public PaymentBuilder Amount(int amount)
    {
        _amount = amount;
        return this;
    }
    
    public PaymentBuilder Currency(string currency)
    {
        _currency = currency;
        return this;
    }
    
    public PaymentBuilder Card(PaymentCard card)
    {
        _card = card;
        return this;
    }
    
    public Payment Build()
        => new(_amount, _currency, _card);
}