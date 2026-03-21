FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["SmartphoneStore.Api/SmartphoneStore.Api.csproj", "SmartphoneStore.Api/"]
COPY ["SmartphoneStore.Orchestrator/SmartphoneStore.Orchestrator.csproj", "SmartphoneStore.Orchestrator/"]
COPY ["SmartphoneStore.Dal/SmartphoneStore.Dal.csproj", "SmartphoneStore.Dal/"]
COPY ["SmartphoneStore.Model/SmartphoneStore.Model.csproj", "SmartphoneStore.Model/"]
RUN dotnet restore "SmartphoneStore.Api/SmartphoneStore.Api.csproj"

COPY . .
WORKDIR "/src/SmartphoneStore.Api"
RUN dotnet build "SmartphoneStore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartphoneStore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartphoneStore.Api.dll"]