#!/bin/bash

cd ../../../../
docker build . -f sources/apps/Api/Dockerfile --target ef -t migrate:latest
