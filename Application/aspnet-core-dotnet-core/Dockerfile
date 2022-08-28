#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["aspnet-core-dotnet-core.csproj", "aspnet-core-dotnet-core/"]
RUN dotnet restore "aspnet-core-dotnet-core/aspnet-core-dotnet-core.csproj"
WORKDIR "/src/aspnet-core-dotnet-core"
COPY . .

RUN dotnet build "aspnet-core-dotnet-core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "aspnet-core-dotnet-core.csproj" -c Release -o /app/publish -r linux-64 /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnet-core-dotnet-core.dll"]