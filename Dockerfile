# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY blazor_project.csproj ./
RUN dotnet restore blazor_project.csproj

COPY . .
RUN dotnet publish blazor_project.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
# Render injects $PORT; default 8080 if running ad-hoc.
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "blazor_project.dll"]
