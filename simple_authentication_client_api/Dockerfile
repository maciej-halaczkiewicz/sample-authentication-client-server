#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["simple_authentication_client_api/simple_authentication_client_api.csproj", "simple_authentication_client_api/"]
COPY ["simple_authentication_client_application/simple_authentication_client_application.csproj", "simple_authentication_client_application/"]
COPY ["simple_authentication_client_domain/simple_authentication_client_domain.csproj", "simple_authentication_client_domain/"]
COPY ["simple_authentication_client_infrastructure/simple_authentication_client_infrastructure.csproj", "simple_authentication_client_infrastructure/"]
RUN dotnet restore "./simple_authentication_client_api/./simple_authentication_client_api.csproj"
COPY . .
WORKDIR "/src/simple_authentication_client_api"
RUN dotnet build "./simple_authentication_client_api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./simple_authentication_client_api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "simple_authentication_client_api.dll"]