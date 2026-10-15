FROM mcr.microsoft.com/dotnet/aspnet:10.0.13-noble AS base
WORKDIR /app
EXPOSE 5001 5000

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:10.0.401-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

RUN dotnet restore
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef --version 10.0.0
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
