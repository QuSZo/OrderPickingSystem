ARG PYTHON_VERSION=3.14-slim
ARG LOCAL_APP_DIR=sources/apps/AlphaBot
ARG APP_NAME=AlphaBot

FROM python:${PYTHON_VERSION} as release
ARG PYTHON_VERSION
ARG LOCAL_APP_DIR
ARG APP_NAME

COPY ./${LOCAL_APP_DIR}/requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY ./${LOCAL_APP_DIR} /${APP_NAME}
WORKDIR /${APP_NAME}

CMD ["python", "main.py", "--simulate", "true", "--mqttBrokerUrl", "mqtt-broker"]