#!/bin/bash

docker compose -f ../../../compose/compose.yml up --abort-on-container-exit; docker compose -f ../../../compose/compose.yml down
