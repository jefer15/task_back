# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos del proyecto
COPY . ./

# Restaurar dependencias
RUN dotnet restore

# Compilar la aplicación en modo Release
RUN dotnet publish -c Release -o /publish

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos de la etapa anterior
COPY --from=build /publish .

# Exponer el puerto en el contenedor
EXPOSE 8080

# Ejecutar la aplicación
CMD ["dotnet", "task_back.dll"]
