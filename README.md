# Payment Gateway

Simple Payment Gateway sample project using:

 - [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html)
 - [CQRS](https://martinfowler.com/bliki/CQRS.html)
 - [DDD tactical patterns](https://thedomaindrivendesign.io/what-is-tactical-design/)
 - [Hexagonal architecture](https://alistair.cockburn.us/hexagonal-architecture/)
 - [ASP.NET 6 Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0)

This project is only for demonstration purposes and should not be considered production-ready. 

The solution includes the Payment Gateway as well an Acquiring Bank simulator for development and testing purposes.

## Run using Docker Compose

You can run whole application using [docker compose](https://docs.docker.com/compose/) from root folder:
```
docker-compose up -d
```

It will create following services:

- Acquiring Bank Simulator
- Payment Gateway API


## How to use

The Payment Gateway will allow you to interact with the API via Swagger UI. Check the [Open API specification](https://github.com/bymyslf/payment-gateway/blob/main/PaymentGateway/src/PaymentGateway.Api/docs/contract.yml).

Type the following URL in your browser:

```
http://localhost:5270/swagger 
```


## Run tests

If you want to run the automated tests you can do it using [docker compose](https://docs.docker.com/compose/) from the root folder:
```
docker-compose -f .\docker-compose-tests.yml up
```

This will run all the tests.

You can also target only some tests:

```
docker-compose -f .\docker-compose-tests.yml up [payment-gateway-unittests | payment-gateway-apitests]
```

## Improvements

- Replace InMemory eevent store with [EventStoreDB](https://www.eventstore.com/eventstoredb) | [SQLStreamStore](https://github.com/SQLStreamStore/SQLStreamStore) | [Marten](https://martendb.io/)
- Implement idempotency (see [API contract](https://github.com/bymyslf/payment-gateway/blob/main/PaymentGateway/src/PaymentGateway.Api/docs/contract.yml#L25))
- Improve telemetry ([OTEL](https://opentelemetry.io/))
- Anonymize PII (Personal Identifiable Information)
- Snapshotting sample
- Projections sample


