#!/bin/bash

cd ../../../../
docker build . -f sources/apps/Web/Dockerfile --target release --build-arg VITE_API_URL="http://localhost:8080/" -t web:latest
