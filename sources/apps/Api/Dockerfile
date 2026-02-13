ARG DOTNET_VERSION=10.0
ARG LOCAL_APP_DIR=sources/apps/Api
ARG APP_NAME=Api

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG DOTNET_VERSION
ARG LOCAL_APP_DIR
ARG APP_NAME

COPY ./${LOCAL_APP_DIR} /${APP_NAME}
RUN dotnet publish -c Release /${APP_NAME}/${APP_NAME}.slnx -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS release
ARG DOTNET_VERSION
ARG LOCAL_APP_DIR
ARG APP_NAME

WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Api.dll"]
