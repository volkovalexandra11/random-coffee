﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RandomCoffeeServer.csproj", "./"]
RUN dotnet restore "RandomCoffeeServer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "RandomCoffeeServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RandomCoffeeServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG APPLICATION_MODES
ENV APPLICATION_MODES=$APPLICATION_MODES
ENTRYPOINT ["dotnet", "RandomCoffeeServer.dll"]
