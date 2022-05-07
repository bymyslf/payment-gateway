using PaymentGateway.Core.Domain.Common;

namespace PaymentGateway.Core.Domain;

public class PaymentCard : Entity
{
    public PaymentCard(PaymentCardNumber number, PaymentCardType type, string holderName, int expireMonth, int expireYear, int cvv)
    {
        Guard.Against<ArgumentException>(number is null, "Payment card number is required");
        Guard.Against<ArgumentException>(string.IsNullOrWhiteSpace(holderName), "Payment card holder name is required");
        Guard.Against<ArgumentException>(CvvHasNotThreeDigits(cvv), "Payment card CVV requires 3 digits");
        Guard.Against<ArgumentException>(HasExpired(expireMonth, expireYear), "Payment card has expired");

        Id = Guid.NewGuid();
        Type = type;
        HolderName = holderName;
        Number = number;
        ExpireMonth = expireMonth;
        ExpireYear = expireYear;
        Cvv = cvv;
    }

    public PaymentCardType Type { get; }
    
    public string HolderName { get; }
    
    public PaymentCardNumber Number { get; }
    
    public int ExpireMonth { get; }
    
    public int ExpireYear { get; }
    
    public int Cvv { get; }

    private static bool HasExpired(int expireMonth, int expireYear)
    {
        if (expireMonth < 0 || expireMonth > 12)
            return true;
        
        if (expireYear < DateTime.Today.Year)
            return true;
        
        return expireMonth < DateTime.Today.Month;
    }

    private static bool CvvHasNotThreeDigits(int cvv)
        => cvv < 100 || cvv > 999;
}
