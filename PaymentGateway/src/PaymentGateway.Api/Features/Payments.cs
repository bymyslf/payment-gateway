using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using PaymentGateway.Core.UseCases.Common;
using PaymentGateway.Core.UseCases.GetPaymentDetails;
using PaymentGateway.Core.UseCases.PaymentRequest;

namespace PaymentGateway.Api.Features;

internal static class Payments
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        app.MapPost("/payments", HandlePaymentRequest);
        app.MapGet("/payments/{payment-id:Guid}", HandleGetPayment);
        
        async Task<IResult> HandlePaymentRequest(
            [FromServices] ICommandHandler<PaymentRequest, PaymentRequestResult> handler,
            [FromBody] PaymentRequestDto request,
            CancellationToken cancellationToken
        )
        {
            if (!MiniValidator.TryValidate(request, out var errors))
            {
                Results.Problem(new ProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "One or more validation errors occurred.",
                    Extensions =
                    {
                        ["errors"] = errors
                    }
                });
            }
            
            var command = PaymentRequestDto.ToCommand(request);
            var result = await handler.Handle(command, cancellationToken);
            return Results.Created($"/payments/{result.Id}", PaymentRequestResponseDto.FromResult(result));
        }

        async Task<IResult> HandleGetPayment(
            [FromServices] IQueryHandler<GetPayment, GetPaymentResult> handler,
            [FromRoute(Name = "payment-id")] Guid paymentId,
            CancellationToken cancellationToken
        )
        {
            if (paymentId == Guid.Empty)
            {
                Results.Problem(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Payment ID cannot be empty"
                });
            }
            
            var query = new GetPayment(paymentId);
            var result = await handler.Handle(query, cancellationToken);
            if (result is null)
            {
                Results.Problem(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Payment not found"
                });
            }
            
            return Results.Ok(GetPaymentResponseDto.FromDomain(result));
        }
    }
}

public record PaymentRequestDto(
    [Required] int Amount, 
    [Required][StringLength(3)] string Currency, 
    [Required] PaymentRequestCardDto Card)
{
    public static PaymentRequest ToCommand(PaymentRequestDto dto)
        => new(
            dto.Amount,
            dto.Currency,
            PaymentRequestCardDto.ToCommand(dto.Card)
        );
}

public record PaymentRequestCardDto(
    [Required] string Type, 
    [Required] string HolderName, 
    [Required] string Number, 
    [Required][Range(1, 12)] int ExpireMonth,
    [Required] int ExpireYear,
    [Required][Range(100, 999)] int Cvv)
{
    public static PaymentRequestCard ToCommand(PaymentRequestCardDto dto)
        => new(
            dto.Type,
            dto.HolderName,
            dto.Number,
            dto.ExpireMonth,
            dto.ExpireYear,
            dto.Cvv
        );
}

public record PaymentRequestResponseDto(string Id, string Status)
{
    public static PaymentRequestResponseDto FromResult(PaymentRequestResult result)
        => new(
            result.Id.ToString(),
            result.Status.ToString()
        );
}

public record GetPaymentResponseDto(string Id, int Amount, string Currency, string Status, DateTime ProcessedOn, GetPaymentResponseCardDto Card)
{
    public static GetPaymentResponseDto FromDomain(GetPaymentResult domain)
        => new(
            domain.Id.ToString(),
            domain.Amount,
            domain.Currency,
            domain.Status.ToString(),
            domain.ProcessedOn,
            GetPaymentResponseCardDto.FromDomain(domain.Card)
        );
}

public record GetPaymentResponseCardDto(string Type, string HolderName, string Number, int ExpireMonth, int ExpireYear, int Cvv)
{
    public static GetPaymentResponseCardDto FromDomain(GetPaymentResultCard domain)
        => new(
            domain.Type,
            domain.HolderName,
            domain.Number,
            domain.ExpireMonth,
            domain.ExpireYear,
            domain.Cvv
        );
}