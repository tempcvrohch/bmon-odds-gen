FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /bmon-odds

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /bmon-odds
COPY --from=build-env /bmon-odds/out .
ENTRYPOINT ["dotnet", "Org.BmonOddsGen.dll"]