# Multi-stage build otimizado
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY ["TaskApi.csproj", "./"]
RUN dotnet restore "TaskApi.csproj"

# Copia código e realiza o build
COPY . .
RUN dotnet build "TaskApi.csproj" -c Release -o /app/build
RUN dotnet publish "TaskApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskApi.dll"]