FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base

# Instala pacotes necessários (sem prompts e com limpeza para reduzir tamanho)
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       ffmpeg \
       libgdiplus \
       libc6-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hackton.Consumer/Hackton.Consumer.csproj", "Hackton.Consumer/"]
RUN dotnet restore "./Hackton.Consumer/Hackton.Consumer.csproj"
COPY . .
WORKDIR "/src/Hackton.Consumer"
RUN dotnet build "./Hackton.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hackton.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hackton.Consumer.dll"]
