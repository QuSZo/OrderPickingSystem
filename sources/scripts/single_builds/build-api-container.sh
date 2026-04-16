#!/bin/bash

cd ../../../
docker build . -f sources/apps/Api/Dockerfile --target release --build-arg PYTHONNET_PYDLL=/usr/lib/x86_64-linux-gnu/libpython3.12.so -t api:latest
