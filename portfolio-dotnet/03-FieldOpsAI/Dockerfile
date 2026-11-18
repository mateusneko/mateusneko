# Build: docker build -f 03-FieldOpsAI/Dockerfile -t fieldopsai .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish 03-FieldOpsAI/FieldOpsAI.Api/FieldOpsAI.Api.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FieldOpsAI.Api.dll"]
