using System;
using PaymentGateway.Api.Features;
using PaymentGateway.Core.Domain;

namespace PaymentGateway.ApiTests.Builders;

internal class PaymentRequestCardDtoBuilder
{
    private const string DefaultType = "MasterCard";
    private const string DefaultHolderName = "Test Run";
    private const string DefaultNumber = "5436031030606378";
    private const int DefaultExpireMonth = 9;
    private static readonly int DefaultExpireYear = DateTime.Today.Year + 1;
    private const int DefaultCvv = 256;
    
    public static PaymentRequestCardDtoBuilder PaymentRequestCardDto()
        => new();
    
    private string _type;
    private string _holderName;
    private string _number;
    private int _expireMonth;
    private int _expireYear;
    private int _cvv;

    private PaymentRequestCardDtoBuilder()
    { }
    
    public PaymentRequestCardDtoBuilder TestValues()
    {
        Type(DefaultType);
        HolderName(DefaultHolderName);
        Number(DefaultNumber);
        ExpireMonth(DefaultExpireMonth);
        ExpireYear(DefaultExpireYear);
        Cvv(DefaultCvv);
        return this;
    } 
    
    public PaymentRequestCardDtoBuilder Number(string number)
    {
        _number = number;
        return this;
    }
    
    public PaymentRequestCardDtoBuilder Type(string type)
    {
        _type = type;
        return this;
    }
    
    public PaymentRequestCardDtoBuilder HolderName(string holderName)
    {
        _holderName = holderName;
        return this;
    }

    public PaymentRequestCardDtoBuilder ExpireMonth(int expireMonth)
    {
        _expireMonth = expireMonth;
        return this;
    }
    
    public PaymentRequestCardDtoBuilder ExpireYear(int expireYear)
    {
        _expireYear = expireYear;
        return this;
    }
    
    public PaymentRequestCardDtoBuilder Cvv(int cvv)
    {
        _cvv = cvv;
        return this;
    }
    
    public PaymentRequestCardDto Build()
        => new PaymentRequestCardDto(_type, _holderName, _number, _expireMonth, _expireYear, _cvv);
}