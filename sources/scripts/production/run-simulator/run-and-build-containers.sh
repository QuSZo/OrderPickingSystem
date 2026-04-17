#!/bin/bash

VITE_API_URL="http://172.30.0.199:8080/" docker compose -f ../../../compose/compose.yml --profile full --profile simulator up --build;
