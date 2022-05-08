using System.Net;
using System.Threading.Tasks;
using PaymentGateway.ApiTests.Common.Extensions;
using Shouldly;
using Xunit;
using static PaymentGateway.ApiTests.Builders.PaymentRequestDtoBuilder;

namespace PaymentGateway.ApiTests;

public class PaymentsTests : IClassFixture<PaymentApiFixture>
{
    private readonly PaymentApiFixture _fixture;

    public PaymentsTests(PaymentApiFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Creating_payment_returns_payment_id_with_status()
    {
        var client = _fixture.CreateClient();

        var paymentRequest = PaymentRequestDto()
            .TestValues()
            .Build();

        var response = await client.CreateAsync("/payments", paymentRequest);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.ShouldMatchSnapshot("$.id");
    }

    [Fact]
    public async Task Get_payment_returns_all_payment_details_from_previously_created_payment()
    {
        var client = _fixture.CreateClient();

        var paymentRequest = PaymentRequestDto()
            .TestValues()
            .Build();

        var create = await client.CreateAsync("/payments", paymentRequest);
        create.EnsureSuccessStatusCode();
        var result = await create.ReadAsync<PaymentRequestResult>();

        var response = await client.GetAsync($"/payments/{result.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.ShouldMatchSnapshot("$.id", "$.processed_on");
    }
    
    private record PaymentRequestResult(string Id, string Status);
}
    