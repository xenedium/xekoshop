# Build env
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /source

COPY *.csproj .
RUN dotnet restore  -r linux-x64

COPY . .
RUN dotnet publish -c Release -o /app -r linux-x64 --self-contained false --no-restore

# Runtime env
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64
WORKDIR /app

# Copy build files from the build-env
COPY --from=build-env /app .
ENTRYPOINT ["./xekoshop"]