#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/layoutlovers.Migrator/layoutlovers.Migrator.csproj", "src/layoutlovers.Migrator/"]
COPY ["src/layoutlovers.EntityFrameworkCore/layoutlovers.EntityFrameworkCore.csproj", "src/layoutlovers.EntityFrameworkCore/"]
COPY ["src/layoutlovers.Core/layoutlovers.Core.csproj", "src/layoutlovers.Core/"]
COPY ["src/layoutlovers.Core.Shared/layoutlovers.Core.Shared.csproj", "src/layoutlovers.Core.Shared/"]
RUN dotnet restore "src/layoutlovers.Migrator/layoutlovers.Migrator.csproj"
COPY . .
WORKDIR "/src/src/layoutlovers.Migrator"
RUN dotnet build "layoutlovers.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "layoutlovers.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "layoutlovers.Migrator.dll"]
