import paho.mqtt.client as mqtt
import json

class MQTTClient:

    def __init__(self):
        self.client = mqtt.Client()

        self.client.on_connect = self.on_connect
        self.client.on_message = self.on_message

    def connect(self):
        self.client.connect("localhost", 1883, 60)
        self.client.loop_start()

    def on_connect(self, client, userdata, flags, rc):
        print("Connected to broker")
        client.subscribe("robot/command")
        print("Subscribed to topic")

    def on_message(self, client, userdata, msg):
        payload = json.loads(msg.payload.decode())
        print(f"Received message: {payload}")