FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["src/GovernmentAdviceApi/GovernmentAdviceApi.csproj", "src/GovernmentAdviceApi/"]
RUN dotnet restore "src/GovernmentAdviceApi/GovernmentAdviceApi.csproj"
COPY . .
WORKDIR "/src/src/GovernmentAdviceApi"
RUN dotnet build "GovernmentAdviceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GovernmentAdviceApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GovernmentAdviceApi.dll"]
