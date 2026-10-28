# ---------- Stage 1: build ----------
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Build.props ./
COPY src/Application/Application.csproj src/Application/
COPY src/TubeTrans/TubeTrans.csproj src/TubeTrans/
RUN dotnet restore src/TubeTrans/TubeTrans.csproj -p:UseAppHost=false

COPY src/ src/
RUN dotnet publish src/TubeTrans/TubeTrans.csproj -c Release -o /app --no-restore -p:UseAppHost=false

# ---------- Stage 2: runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app ./

LABEL org.opencontainers.image.source="https://github.com/WilmeGM/TubeTrans" \
      org.opencontainers.image.description="Get the transcript of any public YouTube video as plain text."

# Run as a non-root user (provided by the official image).
USER $APP_UID

# The official image listens on port 8080 by default.
EXPOSE 8080

ENTRYPOINT ["dotnet", "TubeTrans.dll"]