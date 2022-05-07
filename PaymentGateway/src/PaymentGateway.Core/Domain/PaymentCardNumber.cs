using PaymentGateway.Core.Domain.Common;
using ValueObject = PaymentGateway.Core.Domain.Common.ValueObject;

namespace PaymentGateway.Core.Domain;

public class PaymentCardNumber : ValueObject
{
    public PaymentCardNumber(string number)
    {
        Guard.Against<ArgumentException>(!IsNumberValid(number), "Payment card number is invalid");
        Value = number;
    }

    public string Value { get; }

    private static bool IsNumberValid(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return false;
        
        //TODO: most cards 16 is the correct length but not all
        if (number.Length != 16)
            return false;

        return CheckDigit(number);

        static bool CheckDigit(string number)
        {
            int length = number.Length;
            int sum = 0;
            int parity = (length - 2) % 2;
            
            for (int i = length - 1; i >= 0; i--)
            {
                int digit = int.Parse(number[i].ToString());

                if (i % 2 == parity)
                    digit *= 2;

                if (digit > 9)
                    digit -= 9;

                sum += digit;
            }

            return sum % 10 == 0;
        }
    }
    
    public static implicit operator string(PaymentCardNumber number)
        => number.Value;

    public static explicit operator PaymentCardNumber(string number)
        => new(number);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}