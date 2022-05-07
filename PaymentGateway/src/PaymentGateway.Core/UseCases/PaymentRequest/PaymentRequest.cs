using PaymentGateway.Core.Domain;

namespace PaymentGateway.Core.UseCases.PaymentRequest;

public record PaymentRequest(int Amount, string Currency, PaymentRequestCard Card);

public record PaymentRequestCard(string Type, string HolderName, string Number, int ExpireMonth, int ExpireYear, int Cvv);

public record PaymentRequestResult(Guid Id, PaymentStatus Status);
    