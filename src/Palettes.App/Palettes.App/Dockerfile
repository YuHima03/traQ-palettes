# Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy and restore dotnet projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
WORKDIR /src
COPY ["src/Palettes.App/Palettes.App/Palettes.App.csproj", "src/Palettes.App/Palettes.App/"]
COPY ["src/Palettes.App/Palettes.App.Client/Palettes.App.Client.csproj", "src/Palettes.App/Palettes.App.Client/"]
COPY ["src/Palettes.Domain/Palettes.Domain.csproj", "src/Palettes.Domain/"]
COPY ["src/Palettes.Utils/Palettes.Utils.csproj", "src/Palettes.Utils/"]
RUN dotnet restore "./src/Palettes.App/Palettes.App/Palettes.App.csproj"
COPY . .

# Generate css files with TailwindCSS-CLI.
FROM node:lts-alpine AS pre_build
WORKDIR /src
COPY --from=restore /src .
WORKDIR "/src"
RUN npm i
RUN npx @tailwindcss/cli -i "./src/Palettes.App/Palettes.App/Styles/app.css" -o "./src/Palettes.App/Palettes.App/wwwroot/css/app.css"

# Build dotnet project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY --from=pre_build /src /src
WORKDIR "/src/src/Palettes.App/Palettes.App"
RUN dotnet build "./Palettes.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish dotnet project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Palettes.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Palettes.App.dll"]
