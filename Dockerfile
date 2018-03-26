FROM microsoft/dotnet:1-sdk

WORKDIR /app

COPY ID4iClientSample/* /app/ 
RUN dotnet restore 
RUN dotnet build
