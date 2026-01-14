# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy solution file and project files
COPY ZFinance.slnx .
COPY ZFinance.Core/ZFinance.Core.csproj ZFinance.Core/
COPY ZFinance.WebAPI/ZFinance.WebAPI.csproj ZFinance.WebAPI/

# Restore dependencies using the solution
RUN dotnet restore ZFinance.slnx

# Copy the rest of the application source code
COPY ZFinance.Core/ ZFinance.Core/
COPY ZFinance.WebAPI/ ZFinance.WebAPI/

# Publish the WebAPI project
RUN dotnet publish ZFinance.WebAPI/ZFinance.WebAPI.csproj -c Release --no-restore -o /app_publish_output

# Stage 2: Run the application in a minimal runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Use the "app" non-root user by default for enhanced security
USER $APP_UID

# Copy the published output from the build stage
COPY --from=build /app_publish_output .

# Expose port 8080, which is the default for ASP.NET Core apps in official images starting with .NET 8
EXPOSE 8080

ENTRYPOINT ["dotnet", "ZFinance.WebAPI.dll"]
