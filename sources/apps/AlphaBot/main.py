import time
from mqtt_client import MQTTClient

mqtt_client = MQTTClient()
mqtt_client.connect()

while True:
    time.sleep(1)