#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/layoutlovers.Web.Host/layoutlovers.Web.Host.csproj", "src/layoutlovers.Web.Host/"]
COPY ["src/layoutlovers.Web.Core/layoutlovers.Web.Core.csproj", "src/layoutlovers.Web.Core/"]
COPY ["src/layoutlovers.Application/layoutlovers.Application.csproj", "src/layoutlovers.Application/"]
COPY ["src/layoutlovers.Application.Shared/layoutlovers.Application.Shared.csproj", "src/layoutlovers.Application.Shared/"]
COPY ["src/layoutlovers.Core.Shared/layoutlovers.Core.Shared.csproj", "src/layoutlovers.Core.Shared/"]
COPY ["src/layoutlovers.Core/layoutlovers.Core.csproj", "src/layoutlovers.Core/"]
COPY ["src/layoutlovers.EntityFrameworkCore/layoutlovers.EntityFrameworkCore.csproj", "src/layoutlovers.EntityFrameworkCore/"]
COPY ["src/layoutlovers.GraphQL/layoutlovers.GraphQL.csproj", "src/layoutlovers.GraphQL/"]
RUN dotnet restore "src/layoutlovers.Web.Host/layoutlovers.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/layoutlovers.Web.Host"
RUN dotnet build "layoutlovers.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "layoutlovers.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "layoutlovers.Web.Host.dll"]
