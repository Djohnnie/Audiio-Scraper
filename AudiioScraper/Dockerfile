FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AudiioScraper.sln", "./"]
COPY ["AudiioScraper.Common/*.csproj", "./AudiioScraper.Common/"]
COPY ["AudiioScraper.Entities/*.csproj", "./AudiioScraper.Entities/"]
COPY ["AudiioScraper.DataAccess/*.csproj", "./AudiioScraper.DataAccess/"]
COPY ["AudiioScraper.Worker/*.csproj", "./AudiioScraper.Worker/"]

RUN dotnet restore
COPY . .
WORKDIR "/src/AudiioScraper.Common"
RUN dotnet build "AudiioScraper.Common.csproj" -c Release -o /app/build
WORKDIR "/src/AudiioScraper.Entities"
RUN dotnet build "AudiioScraper.Entities.csproj" -c Release -o /app/build
WORKDIR "/src/AudiioScraper.DataAccess"
RUN dotnet build "AudiioScraper.DataAccess.csproj" -c Release -o /app/build
WORKDIR "/src/AudiioScraper.Worker"
RUN dotnet build "AudiioScraper.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AudiioScraper.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=Europe/Brussels
ENV DB_CONNECTION_STRING=...
ENV SCRAPER_INTERVAL_IN_SECONDS=...
ENV DOWNLOADER_INTERVAL_IN_SECONDS=...
ENV MUSIC_DOWNLOAD_PATH=...
ENV SFX_DOWNLOAD_PATH=...
ENV MUSIC_API_ADDRESS=...
ENV SFX_API_ADDRESS=...
ENV MUSIC_CDN_BASE_ADDRESS=...
ENV SFX_CDN_BASE_ADDRESS=...
ENV ART_CDN_BASE_ADDRESS=...

ENTRYPOINT ["dotnet", "AudiioScraper.Worker.dll"]