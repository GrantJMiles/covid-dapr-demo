#testconsumer:one
FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY PromiseApi/bin/Release/net6.0/publish/ App/
WORKDIR /App
# ENTRYPOINT ["ls", "."]
ENTRYPOINT ["dotnet", "PromiseApi.dll"]