using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.UseCases.PaymentRequest;
using Shouldly;
using Xunit;
using  static PaymentGateway.UnitTests.Builders.PaymentRequestBuilder;

namespace PaymentGateway.UnitTests.UseCases;

public class PaymentRequestHandlerTests
{
    [Fact]
    public async Task Valid_payment_request_is_sent_to_acquiring_bank()
    {
        var acquiringBank = new AcquiringBankSpy();

        var paymentRequest = PaymentRequest()
            .TestValues()
            .Build();
        
        var handler = new PaymentRequestHandler(new AggregateStoreStub(), acquiringBank, NullLogger<PaymentRequestHandler>.Instance);
        var result = await handler.Handle(paymentRequest, CancellationToken.None);

        acquiringBank.Requests.TryGetValue(result.Id, out Payment payment);

        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Currency.ShouldBe(paymentRequest.Currency);
        payment.Card.HolderName.ShouldBe(paymentRequest.Card.HolderName);
        payment.Card.ExpireMonth.ShouldBe(paymentRequest.Card.ExpireMonth);
        payment.Card.ExpireYear.ShouldBe(paymentRequest.Card.ExpireYear);
        payment.Card.Cvv.ShouldBe(paymentRequest.Card.Cvv);
        payment.Card.Number.Value.ShouldBe(paymentRequest.Card.Number);
        payment.Card.Type.ToString().ShouldBe(paymentRequest.Card.Type);
    }
}