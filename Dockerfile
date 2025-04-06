FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InvestmentManagementService.csproj", "./"]
RUN dotnet restore "InvestmentManagementService.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "InvestmentManagementService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InvestmentManagementService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["appsettings.Docker.json", "./"]

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "InvestmentManagementService.dll"] 