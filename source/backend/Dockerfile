ARG BUILD_CONFIGURATION=Release
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001 5000

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 5.0.0
# Copy everything else and build

WORKDIR /src/api
RUN dotnet build "Pims.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "Pims.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish

# Runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./entrypoint.sh .
RUN chmod +x /app/entrypoint.sh
ENTRYPOINT ["/app/entrypoint.sh"]
