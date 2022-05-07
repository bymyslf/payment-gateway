using System;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.UnitTests.Builders;

internal class PaymentRequestCardBuilder
{
    private const string DefaultType = "MasterCard";
    private const string DefaultHolderName = "Test Run";
    private const string DefaultNumber = "5436031030606378";
    private const int DefaultExpireMonth = 9;
    private static readonly int DefaultExpireYear = DateTime.Today.Year + 1;
    private const int DefaultCvv = 256;
    
    public static PaymentRequestCardBuilder PaymentRequestCard()
        => new();
    
    private string _type;
    private string _holderName;
    private string _number;
    private int _expireMonth;
    private int _expireYear;
    private int _cvv;

    private PaymentRequestCardBuilder()
    { }
    
    public PaymentRequestCardBuilder TestValues()
    {
        Type(DefaultType);
        HolderName(DefaultHolderName);
        Number(DefaultNumber);
        ExpireMonth(DefaultExpireMonth);
        ExpireYear(DefaultExpireYear);
        Cvv(DefaultCvv);
        return this;
    } 
    
    public PaymentRequestCardBuilder Number(string number)
    {
        _number = number;
        return this;
    }
    
    public PaymentRequestCardBuilder Type(string type)
    {
        _type = type;
        return this;
    }
    
    public PaymentRequestCardBuilder HolderName(string holderName)
    {
        _holderName = holderName;
        return this;
    }

    public PaymentRequestCardBuilder ExpireMonth(int expireMonth)
    {
        _expireMonth = expireMonth;
        return this;
    }
    
    public PaymentRequestCardBuilder ExpireYear(int expireYear)
    {
        _expireYear = expireYear;
        return this;
    }
    
    public PaymentRequestCardBuilder Cvv(int cvv)
    {
        _cvv = cvv;
        return this;
    }
    
    public PaymentRequestCard Build()
        => new(_type, _holderName, _number, _expireMonth, _expireYear, _cvv);
}