#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ManagementBook.Api/ManagementBook.Api.csproj", "ManagementBook.Api/"]
COPY ["ManagementBook.Application/ManagementBook.Application.csproj", "ManagementBook.Application/"]
COPY ["ManagementBook.Domain/ManagementBook.Domain.csproj", "ManagementBook.Domain/"]
COPY ["ManagementBook.Infra.Cross/ManagementBook.Infra.Cross.csproj", "ManagementBook.Infra.Cross/"]
COPY ["ManagementBook.Infra.Data/ManagementBook.Infra.Data.csproj", "ManagementBook.Infra.Data/"]
RUN dotnet restore "./ManagementBook.Api/./ManagementBook.Api.csproj"
COPY . .
WORKDIR "/src/ManagementBook.Api"
RUN dotnet build "./ManagementBook.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ManagementBook.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagementBook.Api.dll"]