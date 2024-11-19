# Estágio de base - imagem do ASP.NET 8.0 para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Estágio de build - imagem do SDK .NET 8.0 para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["api_render/api_render.csproj", "api_render/"]
RUN dotnet restore "api_render/api_render.csproj"
COPY . .
WORKDIR "/src/api_render"
RUN dotnet build "api_render.csproj" -c Release -o /app/build

# Estágio de publicação - cria o pacote de release do projeto
FROM build AS publish
RUN dotnet publish "api_render.csproj" -c Release -o /app/publish

# Estágio final - copia os arquivos de publicação e define a imagem de execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api_render.dll"]
