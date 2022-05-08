using Serilog.Context;

namespace PaymentGateway.Api.Middlewares;

public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        {
            return _next.Invoke(context);
        }
    }
}
