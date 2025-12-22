# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY TOTPGuard.Shared/TOTPGuard.Shared.csproj TOTPGuard.Shared/
COPY TOTPGuard.Api/TOTPGuard.Api.csproj TOTPGuard.Api/
RUN dotnet restore TOTPGuard.Api/TOTPGuard.Api.csproj

COPY . .
WORKDIR /src/TOTPGuard.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TOTPGuard.Api.dll"]
