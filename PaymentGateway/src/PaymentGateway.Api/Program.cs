using System.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.FileProviders;
using PaymentGateway.Api.Features;
using PaymentGateway.Api.Json;
using PaymentGateway.Api.Middlewares;
using Serilog;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting web host");

try
{
    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    
    var builder = WebApplication.CreateBuilder(args);

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
    
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Console());

    builder.Services.AddPaymentsInfrastructure()
        .AddPaymentsDomain();

    builder.Services.Configure<JsonOptions>(opt =>
    {
        opt.SerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseMiddleware<RequestLogContextMiddleware>();
    app.UseMiddleware<ErrorHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            c =>
            {
                c.SwaggerEndpoint("/docs/contract.yml", "Contract Specification Payment Gateway");
            });
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "docs")),
        RequestPath = "/docs",
        ServeUnknownFileTypes = true
    });

    app.UseHttpsRedirection();
//app.UseAuthorization();

    app.MapPaymentsEndpoints();

    app.Urls.Add("http://*:5270");
    
    app.Run();

    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}

