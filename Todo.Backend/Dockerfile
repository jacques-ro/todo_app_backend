FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://0.0.0.0:5000
EXPOSE 5000

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
ENTRYPOINT ["dotnet", "Todo.Backend.dll"]
