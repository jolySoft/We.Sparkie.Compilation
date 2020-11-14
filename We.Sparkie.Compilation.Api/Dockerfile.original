FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["We.Sparkie.Compilation.Api/We.Sparkie.Compilation.Api.csproj", "We.Sparkie.Compilation.Api/"]
RUN dotnet restore "We.Sparkie.Compilation.Api/We.Sparkie.Compilation.Api.csproj"
COPY . .
WORKDIR "/src/We.Sparkie.Compilation.Api"
RUN dotnet build "We.Sparkie.Compilation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "We.Sparkie.Compilation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "We.Sparkie.Compilation.Api.dll"]