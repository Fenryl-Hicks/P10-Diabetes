# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers csproj et restaurer les dépendances
COPY ["PatientService/PatientService.csproj", "PatientService/"]
RUN dotnet restore "PatientService/PatientService.csproj"

# Copier tout le code source et compiler
COPY . .
WORKDIR "/src/PatientService"
RUN dotnet build "PatientService.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "PatientService.csproj" -c Release -o /app/publish /p:UseAppHost=false

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

ENTRYPOINT ["dotnet", "PatientService.dll"]
