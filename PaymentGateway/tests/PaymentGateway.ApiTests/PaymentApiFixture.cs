using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PaymentGateway.ApiTests.Common;
using PaymentGateway.Core.UseCases.Common;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.ApiTests;

public class PaymentApiFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IAcquiringBank));
            services.TryAddSingleton<IAcquiringBank, AcquiringBankFake>();
        });

        return base.CreateHost(builder);
    }
}