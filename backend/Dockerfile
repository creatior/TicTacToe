FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

RUN apt-get update && \
    apt-get install -y swi-prolog locales && \
    sed -i '/en_US.UTF-8/s/^# //g' /etc/locale.gen && \
    locale-gen && \
    apt-get clean && rm -rf /var/lib/apt/lists/*
ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire backend directory
COPY . .

# Restore dependencies using the solution file
RUN dotnet restore TicTacToe.sln

# Build and publish the API project (or whichever is your entry point)
RUN dotnet publish TicTacToe.API/TicTacToe.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
COPY --from=build /src/Prolog/main.pl ./Prolog/
ENTRYPOINT ["dotnet", "TicTacToe.API.dll"]
