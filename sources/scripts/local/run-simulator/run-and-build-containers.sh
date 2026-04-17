#!/bin/bash

VITE_API_URL="http://localhost:8080/" docker compose -f ../../../compose/compose.yml --profile full --profile simulator up --build;
