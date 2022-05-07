var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/payments", (PaymentRequest payment) =>
    {
        if (payment.CardNumber.Equals("4275765574319271"))
            return Results.BadRequest("Payment rejected");

        var payoutId = Guid.NewGuid().ToString();
        return Results.Created($"/payments/{payoutId}",new PaymentResponse(payoutId));
    })
    .Produces(201, typeof(PaymentResponse));

app.Urls.Add("http://*:5370");

app.Run();

internal record PaymentRequest(
    int Amount, 
    string Currency, 
    string CardType,  
    string CardNumber, 
    int CardExpireMonth, 
    int CardExpireYear, 
    int CardCvv);

internal record PaymentResponse(string PayoutId);



