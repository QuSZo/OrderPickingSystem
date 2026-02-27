import paho.mqtt.client as mqtt


class MqttConsumer:
    def __init__(self, client: mqtt.Client):
        self.client = client
        self._received_message_handlers = []
        self.topic = None

    def subscribe(self, topic):
        if topic and self.topic is None:
            self.topic = topic
            self.client.subscribe(topic)
            self.client.message_callback_add(topic, self._received_message)

    def add_received_message_handler(self, handler):
        if handler not in self._received_message_handlers:
            self._received_message_handlers.append(handler)

    def remove_received_message_handler(self, handler):
        if handler in self._received_message_handlers:
            self._received_message_handlers.remove(handler)

    def _raise_received_message(self, payload):
        for handler in self._received_message_handlers:
            handler(payload)

    def _received_message(self, client, userdata, message):
        payload = message.payload.decode()
        print(f"Consumed message from {message.topic}: {payload}")
        self._raise_received_message(payload)