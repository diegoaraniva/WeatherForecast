FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WeatherAPI.WebService/WeatherAPI.WebService.csproj", "WeatherAPI.WebService/"] 
COPY ["WeatherAPI.UnitTesting/WeatherAPI.UnitTesting.csproj", "WeatherAPI.UnitTesting/"] 
COPY ["WeatherAPI.Application/WeatherAPI.Application.csproj", "WeatherAPI.Application/"]
COPY ["WeatherAPI.Domain/WeatherAPI.Domain.csproj", "WeatherAPI.Domain/"]
    
 
RUN dotnet restore "WeatherAPI.WebService/WeatherAPI.WebService.csproj"

COPY . .

WORKDIR "/src/WeatherAPI.WebService"
RUN dotnet publish "WeatherAPI.WebService.csproj" -c release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WeatherAPI.WebService.dll"]
