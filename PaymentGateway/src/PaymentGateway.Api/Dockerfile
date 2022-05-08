FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PaymentGateway.sln", "PaymentGateway.sln"]
COPY ["AcquiringBankSimulator/src/AcquiringBankSimulator.Api/AcquiringBankSimulator.Api.csproj", "AcquiringBankSimulator/src/AcquiringBankSimulator.Api/"]
COPY ["PaymentGateway/src/PaymentGateway.Core/PaymentGateway.Core.csproj", "PaymentGateway/src/PaymentGateway.Core/"]
COPY ["PaymentGateway/src/PaymentGateway.Infrastructure/PaymentGateway.Infrastructure.csproj", "PaymentGateway/src/PaymentGateway.Infrastructure/"]
COPY ["PaymentGateway/src/PaymentGateway.Api/PaymentGateway.Api.csproj", "PaymentGateway/src/PaymentGateway.Api/"]
COPY ["PaymentGateway/src/PaymentGateway.Api/docs/*", "PaymentGateway/src/PaymentGateway.Api/docs/"]
COPY ["PaymentGateway/tests/PaymentGateway.UnitTests/PaymentGateway.UnitTests.csproj", "PaymentGateway/tests/PaymentGateway.UnitTests/"]
COPY ["PaymentGateway/tests/PaymentGateway.ApiTests/PaymentGateway.ApiTests.csproj", "PaymentGateway/tests/PaymentGateway.ApiTests/"]
RUN dotnet restore "PaymentGateway.sln"

WORKDIR "/src/PaymentGateway.Api"
COPY . .
RUN dotnet build -c Release -o /app/build

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS unittest
WORKDIR app/unittests
COPY . .
ENTRYPOINT ["dotnet", "test", "PaymentGateway/tests/PaymentGateway.UnitTests/PaymentGateway.UnitTests.csproj"]

FROM build AS apitest
WORKDIR app/apitests
COPY . .
ENTRYPOINT ["dotnet", "test", "PaymentGateway/tests/PaymentGateway.ApiTests/PaymentGateway.ApiTests.csproj"]

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]
