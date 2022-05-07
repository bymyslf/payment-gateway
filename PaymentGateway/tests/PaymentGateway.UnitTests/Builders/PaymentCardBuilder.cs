using System;
using PaymentGateway.Core.Domain;

namespace PaymentGateway.UnitTests.Builders;

internal class PaymentCardBuilder
{
    private const PaymentCardType DefaultType = PaymentCardType.MasterCard;
    private const string DefaultHolderName = "Test Run";
    private const string DefaultNumber = "5436031030606378";
    private const int DefaultExpireMonth = 9;
    private static readonly int DefaultExpireYear = DateTime.Today.Year + 1;
    private const int DefaultCvv = 256;
    
    public static PaymentCardBuilder PaymentCard()
        => new();
    
    private PaymentCardType _type;
    private string _holderName;
    private string _number;
    private int _expireMonth;
    private int _expireYear;
    private int _cvv;

    private PaymentCardBuilder()
    { }

    public PaymentCardBuilder TestValues()
    {
        Type(DefaultType);
        HolderName(DefaultHolderName);
        Number(DefaultNumber);
        ExpireMonth(DefaultExpireMonth);
        ExpireYear(DefaultExpireYear);
        Cvv(DefaultCvv);
        return this;
    }
    
    public PaymentCardBuilder Number(string number)
    {
        _number = number;
        return this;
    }
    
    public PaymentCardBuilder Type(PaymentCardType type)
    {
        _type = type;
        return this;
    }
    
    public PaymentCardBuilder HolderName(string holderName)
    {
        _holderName = holderName;
        return this;
    }

    public PaymentCardBuilder ExpireMonth(int expireMonth)
    {
        _expireMonth = expireMonth;
        return this;
    }
    
    public PaymentCardBuilder ExpireYear(int expireYear)
    {
        _expireYear = expireYear;
        return this;
    }
    
    public PaymentCardBuilder Cvv(int cvv)
    {
        _cvv = cvv;
        return this;
    }
    
    public PaymentCard Build()
    {
        var number = new PaymentCardNumber(_number);
        return new PaymentCard(number, _type, _holderName, _expireMonth, _expireYear, _cvv);
    }
}