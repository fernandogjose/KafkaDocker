FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Src/Kafka.Sender/Kafka.Sender.csproj", "Src/Kafka.Sender/"]
RUN dotnet restore "Src/Kafka.Sender/Kafka.Sender.csproj"
COPY . .
WORKDIR "/src/Src/Kafka.Sender"
RUN dotnet build "Kafka.Sender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka.Sender.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["dotnet", "Kafka.Sender.dll"]