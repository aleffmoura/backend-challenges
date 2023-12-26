FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Tests/Challenge.CLI.Tests.csproj", "Tests/"]
RUN dotnet restore "Tests/Challenge.CLI.Tests.csproj"

COPY . .
WORKDIR "/src/Tests"
RUN dotnet build "Challenge.CLI.Tests.csproj" -c Release -o /app/build

ENTRYPOINT ["dotnet", "test", "/app/build/Challenge.CLI.Tests.dll", "--verbosity", "normal"]
