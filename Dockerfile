# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0.3 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0.3 AS build
WORKDIR /src

# Copy project files and restore to leverage cache
COPY ["SchoolMgmtAPI/SchoolMgmtAPI.csproj", "SchoolMgmtAPI/"]
RUN dotnet restore "SchoolMgmtAPI/SchoolMgmtAPI.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/SchoolMgmtAPI"
RUN dotnet build "SchoolMgmtAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SchoolMgmtAPI.csproj" -c Release -o /app/publish

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Optional: add non-root user
# RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

ENTRYPOINT ["dotnet", "SchoolMgmtAPI.dll"]
