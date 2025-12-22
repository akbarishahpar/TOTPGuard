FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY . .
    RUN dotnet publish TOTPGuard.Client/TOTPGuard.Client.csproj -c Release -o /app/publish

FROM nginx:stable-alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80