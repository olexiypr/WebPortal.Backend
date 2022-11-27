FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7150
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebPortal.WebAPI/WebPortal.WebAPI.csproj", "WebPortal.WebAPI/"]
COPY ["WebPortal.Application/WebPortal.Application.csproj", "WebPortal.Application/"]
COPY ["WebPortal.DAL/WebPortal.Domain/WebPortal.Domain.csproj", "WebPortal.Domain/"]
COPY ["WebPortal.DAL/WebPortal.Persistence/WebPortal.Persistence.csproj", "WebPortal.Persistence/"]
RUN dotnet restore "WebPortal.WebAPI/WebPortal.WebAPI.csproj"
COPY . .
WORKDIR "/src/WebPortal.WebAPI"
RUN dotnet build "WebPortal.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebPortal.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebPortal.WebAPI.dll"]
