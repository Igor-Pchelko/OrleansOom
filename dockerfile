FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.4-focal AS base

#
# Build
#
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Copy sources and dependencies *.csproj
COPY OrleansOom.csproj /build/

# Restore dependencies
RUN cd /build && dotnet restore

# Copy library and sources
COPY / /build/

# Build stage
WORKDIR /build

RUN dotnet publish --no-restore -c Release -o out

#
# Final
#
FROM base AS final
WORKDIR /app/OrleansOom

COPY --from=build /build/out .

ENV SERVICE_USER appuser

RUN adduser $SERVICE_USER --gecos "First Last,RoomNumber,WorkPhone,HomePhone" --disabled-login --disabled-password --force-badname \
 && chown -R $SERVICE_USER:$SERVICE_USER /app/OrleansOom
USER $SERVICE_USER

ENTRYPOINT dotnet OrleansOom.dll
