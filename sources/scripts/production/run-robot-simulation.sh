#!/bin/bash

cd ../../apps/AlphaBot
source .venv/bin/activate
python main.py --simulate true --mqttBrokerUrl 172.30.0.199