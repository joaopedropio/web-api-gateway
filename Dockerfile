FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /build

COPY . .
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /build/WebAPIGateway/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPIGateway.dll"]