# Etapa 1: Compilación
# Usamos la imagen del SDK de .NET 8 para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código fuente y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Ejecución
# Usamos la imagen de ASP.NET, que es más ligera y segura
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto en el que correrá la aplicación
EXPOSE 8080

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "KalkulosCore.dll"]