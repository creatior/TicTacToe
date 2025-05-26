FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "TicTacToe.sln"
RUN dotnet build "TicTacToe.sln" -c Release --no-restore

WORKDIR /src/TicTacToe.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Копируем production appsettings
COPY ./TicTacToe.API/appsettings.json ./appsettings.json

ENTRYPOINT ["dotnet", "TicTacToe.API.dll"]