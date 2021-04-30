# ----------------------------------------------------------------------------------------------------
# This Dockerfile is copied to the project root during deployment.
#
# This is done due to current limitations of the used heroku deploy github action which does not
# work correctly when you don't build from the root directory (e. g. due to heroku cli pushing based
# on the Dockerfile and the action does not provide the path context when invoking cli commands)
# ----------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Todo.Backend/Todo.Backend.csproj", "./"]
RUN dotnet restore "Todo.Backend.csproj"
COPY ./Todo.Backend/ .
WORKDIR "/src/."
RUN dotnet build "Todo.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV HerokuEnvironment=true

CMD ASPNETCORE_URLS=http://+:$PORT dotnet Todo.Backend.dll
