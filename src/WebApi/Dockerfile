#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used for VS debugging on Docker
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/WebApi/MicroserviceTemplate.WebApi.csproj", "src/WebApi/"]
COPY ["src/Application/MicroserviceTemplate.Application.csproj", "src/Application/"]
COPY ["src/Domain/MicroserviceTemplate.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/MicroserviceTemplate.Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/WebApi/MicroserviceTemplate.WebApi.csproj"
COPY . .
WORKDIR "/src/src/WebApi"
RUN dotnet build "MicroserviceTemplate.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceTemplate.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceTemplate.WebApi.dll"]