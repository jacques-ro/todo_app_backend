# ----------------------------------------------------------------------------------------------------
# This Dockerfile is copied to ../Todo.Backend during deployment and replaces the original Dockerfile.
#
# This is done due to current limitations of the used heroku deploy github action where the build 
# context path and the docker file path must be the same.
# ----------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Todo.Backend.csproj", "./"]
RUN dotnet restore "Todo.Backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Todo.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV HerokuEnvironment=true

CMD ASPNETCORE_URLS=http://+:$PORT dotnet backend.dll
