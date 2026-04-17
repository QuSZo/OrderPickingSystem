#!/bin/bash

cd ../../../../
docker build . -f sources/apps/AlphaBot/Dockerfile --target release -t alphabot-simulator:latest
