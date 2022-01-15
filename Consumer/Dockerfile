#testconsumer:one
FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY Consumer/bin/Release/net6.0/publish/ App/
WORKDIR /App
# ENTRYPOINT ["ls", "."]
ENTRYPOINT ["dotnet", "Consumer.dll"]