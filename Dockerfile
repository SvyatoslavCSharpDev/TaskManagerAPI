# 1. Базовый образ для запуска .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Образ с SDK для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManagerAPI.csproj", "./"]
RUN dotnet restore "TaskManagerAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TaskManagerAPI.csproj" -c Release -o /app/build

# 3. Публикация (сборка в готовый файл)
FROM build AS publish
RUN dotnet publish "TaskManagerAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Финальный образ (только необходимое для запуска)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]