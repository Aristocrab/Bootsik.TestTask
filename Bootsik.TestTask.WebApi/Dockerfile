FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files separately for caching restore
COPY ["Bootsik.TestTask.WebApi/Bootsik.TestTask.WebApi.csproj", "Bootsik.TestTask.WebApi/"]
COPY ["Bootsik.TestTask.Logic/Bootsik.TestTask.Logic.csproj", "Bootsik.TestTask.Logic/"]

# Run restore only on csproj files
RUN dotnet restore "Bootsik.TestTask.WebApi/Bootsik.TestTask.WebApi.csproj"

# Copy the rest of the source
COPY . .

WORKDIR "/src/Bootsik.TestTask.WebApi"
RUN dotnet build "./Bootsik.TestTask.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bootsik.TestTask.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bootsik.TestTask.WebApi.dll"]
