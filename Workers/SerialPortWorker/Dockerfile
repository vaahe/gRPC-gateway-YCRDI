FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Workers/SerialPortWorker/SerialPortWorker.csproj", "Workers/SerialPortWorker/"]
RUN dotnet restore "Workers/SerialPortWorker/SerialPortWorker.csproj"
COPY . .
WORKDIR "/src/Workers/SerialPortWorker"
RUN dotnet build "./SerialPortWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SerialPortWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SerialPortWorker.dll"]
