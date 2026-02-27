import json
import ssl
import time
import threading
import paho.mqtt.client as mqtt


class MqttPublisher:
    def __init__(
        self,
        host: str,
        port: int = 1883,
        username: str = None,
        password: str = None,
        client_id: str = None,
        use_tls: bool = False,
        keepalive: int = 60,
    ):
        self.host = host
        self.port = port
        self.keepalive = keepalive
        self.client = mqtt.Client(client_id=client_id)

        if username and password:
            self.client.username_pw_set(username, password)

        if use_tls:
            self.client.tls_set(cert_reqs=ssl.CERT_REQUIRED)
            self.client.tls_insecure_set(False)

        self.client.on_connect = self._on_connect
        self.client.on_disconnect = self._on_disconnect

        self._publishing = False
        self._publish_thread = None

    def _on_connect(self, client, userdata, flags, rc):
        if rc == 0:
            print("Połączono z brokerem MQTT")
        else:
            print(f"Błąd połączenia, kod: {rc}")

    def _on_disconnect(self, client, userdata, rc):
        print("Rozłączono z brokerem MQTT")

    def connect(self):
        self.client.connect(self.host, self.port, self.keepalive)
        self.client.loop_start()

    def disconnect(self):
        self.stop_periodic_publish()
        self.client.loop_stop()
        self.client.disconnect()

    def publish(self, topic: str, payload, qos: int = 0, retain: bool = False):
        if isinstance(payload, (dict, list)):
            payload = json.dumps(payload)

        result = self.client.publish(topic, payload, qos=qos, retain=retain)

        if result.rc != mqtt.MQTT_ERR_SUCCESS:
            print(f"Błąd publikacji do tematu `{topic}`")

    def start_periodic_publish(self, topic: str, interval: float = 1.0):
        """Rozpoczyna publikowanie co `interval` sekund."""
        if self._publishing:
            return

        self._publishing = True

        def _run():
            counter = 0
            while self._publishing:
                message = {
                    "robotName": "robot1",
                    "timestamp": time.time()
                }

                self.publish(topic, message)
                print(f"Wysłano: {message}")

                counter += 1
                time.sleep(interval)

        self._publish_thread = threading.Thread(target=_run, daemon=True)
        self._publish_thread.start()

    def stop_periodic_publish(self):
        """Zatrzymuje publikowanie."""
        self._publishing = False
        if self._publish_thread:
            self._publish_thread.join()