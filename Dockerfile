# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["RTHomePropertyManagement/RTHomePropertyManagement.csproj", "RTHomePropertyManagement/"]
RUN dotnet restore "RTHomePropertyManagement/RTHomePropertyManagement.csproj"
COPY . .
WORKDIR "/src/RTHomePropertyManagement"
RUN dotnet publish "RTHomePropertyManagement.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RTHomePropertyManagement.dll"]
