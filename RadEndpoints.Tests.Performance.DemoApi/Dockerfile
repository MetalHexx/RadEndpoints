﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RadEndpoints.Tests.Performance.DemoApi/RadEndpoints.Tests.Performance.DemoApi.csproj", "RadEndpoints.Tests.Performance.DemoApi/"]
RUN dotnet restore "RadEndpoints.Tests.Performance.DemoApi/RadEndpoints.Tests.Performance.DemoApi.csproj"
COPY . .
WORKDIR "/src/RadEndpoints.Tests.Performance.DemoApi"
RUN dotnet build "RadEndpoints.Tests.Performance.DemoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RadEndpoints.Tests.Performance.DemoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RadEndpoints.Tests.Performance.DemoApi.dll"]
