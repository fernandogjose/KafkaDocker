FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Src/Kafka.Receive/Kafka.Receive.csproj", "Src/Kafka.Receive/"]
RUN dotnet restore "Src/Kafka.Receive/Kafka.Receive.csproj"
COPY . .
WORKDIR "/src/Src/Kafka.Receive"
RUN dotnet build "Kafka.Receive.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka.Receive.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka.Receive.dll"]