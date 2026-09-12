FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./AdminCRWeb.sln
RUN dotnet publish ./AdminCRWeb/AdminCRWeb.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet AdminCRWeb.dll --urls http://0.0.0.0:${PORT:-8080}"]
