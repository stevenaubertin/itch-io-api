# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/ItchIoApi/ItchIoApi.csproj", "src/ItchIoApi/"]
RUN dotnet restore "src/ItchIoApi/ItchIoApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "src/ItchIoApi/ItchIoApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "src/ItchIoApi/ItchIoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the published application
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "ItchIoApi.dll"]
