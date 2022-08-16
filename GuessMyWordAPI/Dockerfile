#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GuessMyWordAPI/GuessMyWordAPI/GuessMyWordAPI.csproj", "GuessMyWordAPI/GuessMyWordAPI/"]
RUN dotnet restore "GuessMyWordAPI/GuessMyWordAPI/GuessMyWordAPI.csproj"
COPY . .
WORKDIR "/src/GuessMyWordAPI/GuessMyWordAPI"
RUN dotnet build "GuessMyWordAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GuessMyWordAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuessMyWordAPI.dll"]