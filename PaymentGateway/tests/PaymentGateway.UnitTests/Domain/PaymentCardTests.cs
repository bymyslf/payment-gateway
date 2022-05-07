using System;
using Shouldly;
using Xunit;
using static PaymentGateway.UnitTests.Builders.PaymentCardBuilder;

namespace PaymentGateway.UnitTests.Domain;

public class PaymentCardTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Payment_card_requires_holder_name(string holderName)
    {
        var card = PaymentCard()
            .TestValues()
            .HolderName(holderName);

        Should.Throw<ArgumentException>(() => card.Build());
    }
    
    [Theory]
    [InlineData(12)]
    [InlineData(1234)]
    public void Payment_card_requires_a_3_digits_cvv(int cvv)
    {
        var card = PaymentCard()
            .TestValues()
            .Cvv(cvv);

        Should.Throw<ArgumentException>(() => card.Build());
    }
    
    [Theory]
    [InlineData(12, 2021)]
    [InlineData(15, 2022)]
    public void Payment_card_cannot_be_expired(int expireMonth, int expireYear)
    {
        var card = PaymentCard()
            .TestValues()
            .ExpireMonth(expireMonth)
            .ExpireYear(expireYear);

        Should.Throw<ArgumentException>(() => card.Build());
    }
}