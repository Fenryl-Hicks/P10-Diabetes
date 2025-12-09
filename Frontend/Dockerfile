# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers csproj et restaurer les dépendances
COPY ["Frontend/Frontend.csproj", "Frontend/"]
RUN dotnet restore "Frontend/Frontend.csproj"

# Copier tout le code source et compiler
COPY . .
WORKDIR "/src/Frontend"
RUN dotnet build "Frontend.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Variable d'environnement pour indiquer qu'on est dans Docker
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "Frontend.dll"]
