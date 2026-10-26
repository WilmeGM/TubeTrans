# ---------- Stage 1: build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Project files first: the restore layer stays cached while only source code changes.
COPY Directory.Build.props ./
COPY src/Application/Application.csproj src/Application/
COPY src/TubeTrans/TubeTrans.csproj src/TubeTrans/
RUN dotnet restore src/TubeTrans/TubeTrans.csproj

# Now the source code, and the publish.
COPY src/ src/
RUN dotnet publish src/TubeTrans/TubeTrans.csproj -c Release -o /app --no-restore

# ---------- Stage 2: runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app ./

# Run as a non-root user (provided by the official image).
USER $APP_UID

# The official image listens on port 8080 by default.
EXPOSE 8080

ENTRYPOINT ["dotnet", "TubeTrans.dll"]