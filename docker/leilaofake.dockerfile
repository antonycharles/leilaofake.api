# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
LABEL Antony Charles
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/LeilaoFake.Me.Api/*.csproj ./src/LeilaoFake.Me.Api/
COPY src/LeilaoFake.Me.Core/*.csproj ./src/LeilaoFake.Me.Core/
COPY src/LeilaoFake.Me.Infra/*.csproj ./src/LeilaoFake.Me.Infra/
COPY src/LeilaoFake.Me.Service/*.csproj ./src/LeilaoFake.Me.Service/
COPY tests/LeilaoFake.Me.Test/*.csproj ./tests/LeilaoFake.Me.Test/
#
RUN dotnet restore
#
# copy everything else and build app
COPY src/LeilaoFake.Me.Api/. ./src/LeilaoFake.Me.Api/
COPY src/LeilaoFake.Me.Core/. ./src/LeilaoFake.Me.Core/
COPY src/LeilaoFake.Me.Infra/. ./src/LeilaoFake.Me.Infra/
COPY src/LeilaoFake.Me.Service/. ./src/LeilaoFake.Me.Service/
COPY tests/LeilaoFake.Me.Test/. ./tests/LeilaoFake.Me.Test/
#
WORKDIR /app/src/LeilaoFake.Me.Api
RUN dotnet publish -c Release -o out 
# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

COPY --from=build /app/src/LeilaoFake.Me.Api/out ./
ENTRYPOINT ["dotnet", "LeilaoFake.Me.Api.dll"]