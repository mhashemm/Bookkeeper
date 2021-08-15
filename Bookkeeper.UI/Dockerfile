#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Bookkeeper.UI/Bookkeeper.UI.csproj", "Bookkeeper.UI/"]
COPY ["Bookkeeper.Logic/Bookkeeper.Logic.csproj", "Bookkeeper.Logic/"]
RUN dotnet restore "Bookkeeper.UI/Bookkeeper.UI.csproj"
COPY . .
WORKDIR "/src/Bookkeeper.UI"
RUN dotnet build "Bookkeeper.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookkeeper.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookkeeper.UI.dll"]