using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.Common;

namespace PaymentGateway.Core.UseCases.GetPaymentDetails;

public record GetPayment(Guid PaymentId);

public record GetPaymentResult(Guid Id, int Amount, string Currency, PaymentStatus Status, DateTime ProcessedOn, GetPaymentResultCard Card)
{
    public static GetPaymentResult FromDomain(Payment domain)
        => new(
            domain.Id,
            domain.Amount,
            domain.Currency,
            domain.Status,
            domain.ProcessedOn,
            GetPaymentResultCard.FromDomain(domain.Card)
        );
}

public record GetPaymentResultCard(string Type, string HolderName, string Number, int ExpireMonth, int ExpireYear, int Cvv)
{
    public static GetPaymentResultCard FromDomain(PaymentCard domain)
        => new(
            domain.Type.ToString(),
            domain.HolderName,
            domain.Number.Value.Mask(),
            domain.ExpireMonth,
            domain.ExpireYear,
            domain.Cvv
        );
}