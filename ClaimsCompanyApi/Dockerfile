FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClaimsCompanyApi.csproj", "."]
RUN dotnet restore "./././ClaimsCompanyApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ClaimsCompanyApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClaimsCompanyApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080;http://+:8081
ENTRYPOINT ["dotnet", "ClaimsCompanyApi.dll"]