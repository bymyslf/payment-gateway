FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AcquiringBankSimulator/src/AcquiringBankSimulator.Api/AcquiringBankSimulator.Api.csproj", "AcquiringBankSimulator.Api/"]
RUN dotnet restore "AcquiringBankSimulator.Api/AcquiringBankSimulator.Api.csproj"

WORKDIR "/src/AcquiringBankSimulator.Api"
COPY ["AcquiringBankSimulator/", "."]
RUN dotnet build "AcquiringBankSimulator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcquiringBankSimulator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcquiringBankSimulator.Api.dll"]

