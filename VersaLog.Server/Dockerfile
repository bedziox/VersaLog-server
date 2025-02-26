FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base  
USER $APP_UID  
WORKDIR /app  
EXPOSE 8080  
EXPOSE 8081  
  
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  
ARG BUILD_CONFIGURATION=Release  
WORKDIR /src  
COPY ["Directory.Packages.props", "."]  
COPY ["VersaLog.Server/VersaLog.Server.csproj", "VersaLog.Server/"]  
RUN dotnet restore "./VersaLog.Server/VersaLog.Server.csproj"  
COPY . .  
WORKDIR "/src/VersaLog.Server"  
  
RUN dotnet tool install --global dotnet-ef  
ENV PATH="${PATH}:/root/.dotnet/tools"  
RUN dotnet ef migrations add InitialMigration  
RUN dotnet ef database update  
  
RUN dotnet build "./VersaLog.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build  
  
FROM build AS publish  
ARG BUILD_CONFIGURATION=Release  
RUN dotnet publish "./VersaLog.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false  
  
FROM base AS final  
WORKDIR /app  
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "VersaLog.Server.dll"]