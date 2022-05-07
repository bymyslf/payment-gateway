using System;
using System.Linq;
using PaymentGateway.Core.Domain;
using PaymentGateway.Core.Domain.Common;
using PaymentGateway.Core.Domain.Events;
using Shouldly;
using Xunit;
using static PaymentGateway.UnitTests.Builders.PaymentBuilder;

namespace PaymentGateway.UnitTests.Domain;

public class PaymentTests
{
    [Fact]
    public void Payment_amount_should_be_positive()
    {
        var payment = Payment()
            .TestValues()
            .Amount(-20);

        Should.Throw<ArgumentException>(() => payment.Build());
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Payment_currency_follows_ISO4217(string currency)
    {
        var payment = Payment()
            .TestValues()
            .Currency(currency);

        Should.Throw<ArgumentException>(() => payment.Build());
    }
    
    [Fact]
    public void Payment_requires_a_card()
    {
        var payment = Payment()
            .TestValues()
            .Card(null);

        Should.Throw<ArgumentException>(() => payment.Build());
    }
    
    [Fact]
    public void Payment_is_created_in_pending_status()
    {
        var payment = Payment()
            .TestValues()
            .Build();

        payment.Status.ShouldBe(PaymentStatus.Pending);
        var @event = AssertAppliedDomainEvent<PaymentRequested>(payment);
        @event.Id.ShouldBe(payment.Id);
        @event.Amount.ShouldBe(payment.Amount);
        @event.Currency.ShouldBe(payment.Currency);
        @event.Card.ShouldBe(payment.Card);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Payment_is_not_approved_without_payout_id(string payoutId)
    {
        var payment = Payment()
            .TestValues()
            .Build();

        Should.Throw<ArgumentException>(() => payment.Approve(payoutId));
    }
    
    [Fact]
    public void Payment_approved_has_payout_id()
    {
        var payment = Payment()
            .TestValues()
            .Build();

        payment.Approve("fbk_12569");
        
        payment.Status.ShouldBe(PaymentStatus.Approved);
        var @event = AssertAppliedDomainEvent<PaymentApproved>(payment);
        @event.Id.ShouldBe(payment.Id);
        @event.PayoutId.ShouldBe(payment.PayoutId);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Payment_cannot_be_declined_without_a_declined__reason(string reason)
    {
        var payment = Payment()
            .TestValues()
            .Build();

        Should.Throw<ArgumentException>(() => payment.Decline(reason));
    }
    
    [Fact]
    public void Payment_is_declined_with_a_declined_reason()
    {
        var payment = Payment()
            .TestValues()
            .Build();

        payment.Decline("lack of funds");
        
        payment.Status.ShouldBe(PaymentStatus.Declined);
        var @event = AssertAppliedDomainEvent<PaymentDeclined>(payment);
        @event.Id.ShouldBe(payment.Id);
        @event.Reason.ShouldBe(payment.DeclinedReason);
    }

    private static T AssertAppliedDomainEvent<T>(Payment payment)
        where T : DomainEvent
    {
        var domainEvent = payment.GetUncommittedChanges().OfType<T>().SingleOrDefault();
        if (domainEvent is null)
            throw new Exception();

        return domainEvent;
    }
}