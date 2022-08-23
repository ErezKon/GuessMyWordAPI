##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GuessMyWordAPI.csproj", "."]
RUN dotnet restore "./GuessMyWordAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GuessMyWordAPI.csproj"
ENTRYPOINT ["dotnet", "run"]