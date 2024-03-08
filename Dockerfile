#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Netflix_Clone.API/Netflix_Clone.API.csproj", "Netflix_Clone.API/"]
COPY ["Netflix_Clone.Application/Netflix_Clone.Application.csproj", "Netflix_Clone.Application/"]
COPY ["Netflix_Clone.Domain/Netflix_Clone.Domain.csproj", "Netflix_Clone.Domain/"]
COPY ["Netflix_Clone.Shared/Netflix_Clone.Shared.csproj", "Netflix_Clone.Shared/"]
COPY ["Netflix_Clone.Infrastructure/Netflix_Clone.Infrastructure.csproj", "Netflix_Clone.Infrastructure/"]
RUN dotnet restore "./Netflix_Clone.API/Netflix_Clone.API.csproj"
COPY . .
WORKDIR "/src/Netflix_Clone.API"
RUN dotnet build "./Netflix_Clone.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Netflix_Clone.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Netflix_Clone.API.dll"]