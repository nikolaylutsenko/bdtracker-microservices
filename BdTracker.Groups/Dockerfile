FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
EXPOSE 7771

CMD [ "--net", "bdtracker" ]

ENV ASPNETCORE_URLS=http://+:7771

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
#COPY BdTracker.sln ./ #its not needed if you will provide all needed packages
COPY ./BdTracker.Groups/BdTracker.Groups.csproj ./BdTracker.Groups/
COPY ./BdTracker.Shared/BdTracker.Shared.csproj ./BdTracker.Shared/
#COPY ./BdTracker.Users/BdTracker.Users.csproj ./BdTracker.Users/        #this project not needed for build groups service
#COPY ./BdTracker.Gateway/BdTracker.Gateway.csproj ./BdTracker.Gateway/

RUN dotnet restore ./BdTracker.Groups/BdTracker.Groups.csproj
COPY . .

WORKDIR /src
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV "PostgreSQL:ConnectionName"="User ID=root;Password=root;Host=192.168.0.2;Port=5432;Database=bdtracker-groups;Connection Lifetime=0;"

ENTRYPOINT ["dotnet", "BdTracker.Groups.dll"]