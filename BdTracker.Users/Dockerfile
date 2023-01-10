FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
EXPOSE 5091

ENV ASPNETCORE_URLS=http://+:5091

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY BdTracker.sln ./
COPY ./BdTracker.Users/BdTracker.Users.csproj ./BdTracker.Users/
COPY ./BdTracker.Shared/BdTracker.Shared.csproj ./BdTracker.Shared/

RUN dotnet restore
COPY . .

WORKDIR /src
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV "PostgreSQL:ConnectionName"="User ID=root;Password=root;Host=172.17.0.2;Port=5432;Database=bdtracker-users;Connection Lifetime=0;"

ENTRYPOINT ["dotnet", "BdTracker.Users.dll"]