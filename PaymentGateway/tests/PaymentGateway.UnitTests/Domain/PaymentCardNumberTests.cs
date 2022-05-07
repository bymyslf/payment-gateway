using System;
using PaymentGateway.Core.Domain;
using Shouldly;
using Xunit;

namespace PaymentGateway.UnitTests.Domain;

public class PaymentCardNumberTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Payment_card_number_cannot_be_empty(string number)
    {
        Should.Throw<ArgumentException>(() => new PaymentCardNumber(number));
    }
    
    [Theory]
    [InlineData("5436031030606")]
    [InlineData("543603103060637859965")]
    public void Payment_card_number_has_16_digits(string number)
    {
        Should.Throw<ArgumentException>(() => new PaymentCardNumber(number));
    }
    
    [Fact]
    public void Payment_card_number_is_valid()
    {
        var expected = "5436031030606378";
        var result = new PaymentCardNumber(expected);
        result.Value.ShouldBe(expected);
    }
}