ARG NODE_VERSION=24
ARG LOCAL_APP_DIR=sources/apps/Web
ARG APP_NAME=Web

FROM node:${NODE_VERSION}-alpine AS build
ARG LOCAL_APP_DIR
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}

WORKDIR /app
COPY ./${LOCAL_APP_DIR}/package.json ./${LOCAL_APP_DIR}/package-lock.json ./
RUN npm install
COPY ./${LOCAL_APP_DIR} ./
RUN npm run build

FROM nginx:alpine AS release
ARG LOCAL_APP_DIR

RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist /usr/share/nginx/html
COPY ./${LOCAL_APP_DIR}/nginx.conf /etc/nginx/conf.d/default.conf

CMD ["nginx", "-g", "daemon off;"]