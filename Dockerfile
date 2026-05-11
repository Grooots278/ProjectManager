FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Domain/ProjectManager.Domain.csproj Domain/
COPY src/Application/ProjectManager.Application.csproj Application/
COPY src/Infrastructure/ProjectManager.Infrastructure.csproj Infrastructure/
COPY src/WebApi/ProjectManager.WebApi.csproj WebApi/
RUN dotnet restore WebApi/ProjectManager.WebApi.csproj

COPY src/ .
RUN dotnet publish WebApi/ProjectManager.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/projectmanager.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectManager.WebApi.dll"]