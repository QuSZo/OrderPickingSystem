ARG DOTNET_VERSION=10.0
ARG LOCAL_APP_DIR=sources/apps/Api
ARG APP_NAME=Api

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS ef
ARG DOTNET_VERSION
ARG LOCAL_APP_DIR
ARG APP_NAME

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ./${LOCAL_APP_DIR} /${APP_NAME}
WORKDIR /${APP_NAME}/src

RUN dotnet restore
RUN dotnet build

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
ARG PYTHONNET_PYDLL=/usr/lib/x86_64-linux-gnu/libpython3.12.so

RUN apt-get update && apt-get install -y python3 python3-dev python3-pip python3-venv && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY ./${LOCAL_APP_DIR}/requirements.txt .

RUN python3 -m venv /opt/venv
ENV PATH="/opt/venv/bin:$PATH"

RUN pip install -r requirements.txt

ENV PYTHONNET_PYDLL=${PYTHONNET_PYDLL}

COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Api.dll"]
