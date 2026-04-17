#!/bin/bash

cd ../../../../
docker build . -f sources/apps/Web/Dockerfile --target release --build-arg VITE_API_URL="http://172.30.0.199:8080/" -t web:latest
