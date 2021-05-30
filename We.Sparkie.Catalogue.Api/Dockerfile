#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.300-buster-slim AS build
WORKDIR /src
COPY ["We.Sparkie.Compilation.Api/We.Sparkie.Catalogue.Api.csproj", "We.Sparkie.Catalogue.Api/"]
RUN dotnet restore "We.Sparkie.Catalogue.Api/We.Sparkie.Catalogue.Api.csproj"
COPY . .
WORKDIR "/src/We.Sparkie.Compilation.Api"
RUN dotnet build "We.Sparkie.Catalogue.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "We.Sparkie.Catalogue.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "We.Sparkie.Catalogue.Api.dll"]