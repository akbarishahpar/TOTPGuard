# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY TOTPGuard.Shared/TOTPGuard.Shared.csproj TOTPGuard.Shared/
COPY TOTPGuard.Client/TOTPGuard.Client.csproj TOTPGuard.Client/
RUN dotnet restore TOTPGuard.Client/TOTPGuard.Client.csproj

COPY . .
WORKDIR /src/TOTPGuard.Client
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "TOTPGuard.Client.dll"]