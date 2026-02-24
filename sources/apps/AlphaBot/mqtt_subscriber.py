import json
import ssl
import time
import threading
import paho.mqtt.client as mqtt


class ControlledPublisher:
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
        self.client.on_message = self._on_message

        self._publishing = False
        self._publish_thread = None

        self.control_topic = "robot/command"
        self.data_topic = "robot/status"

    # ---------------- MQTT CALLBACKS ---------------- #

    def _on_connect(self, client, userdata, flags, rc):
        if rc == 0:
            print("Połączono z Mosquitto")
            client.subscribe(self.control_topic)
            print(f"Subskrypcja: {self.control_topic}")
        else:
            print(f"Błąd połączenia: {rc}")

    def _on_disconnect(self, client, userdata, rc):
        print("Rozłączono z brokerem")

    def _on_message(self, client, userdata, msg):
        payload = msg.payload.decode()
        print(f"Odebrano: {payload}")

        try:
            data = json.loads(payload)
        except:
            print("Niepoprawny JSON")
            return

        commands = data.get("commands")

        if commands == "start":
            self.start_periodic_publish()

        elif commands == "stop":
            self.stop_periodic_publish()

    # ---------------- LOGIKA PUBLIKACJI ---------------- #

    def start_periodic_publish(self, interval: float = 1.0):
        if self._publishing:
            print("Już publikuję")
            return

        print("Start publikacji co 1 sekundę")
        self._publishing = True

        def _run():
            while self._publishing:
                message = {
                    "robotName": "robot1",
                    "timestamp": time.time(),
                    "status": "running"
                }

                self.publish(self.data_topic, message)
                print(f"Wysłano: {message}")
                time.sleep(interval)

        self._publish_thread = threading.Thread(target=_run, daemon=True)
        self._publish_thread.start()

    def stop_periodic_publish(self):
        if not self._publishing:
            return

        print("Zatrzymuję publikację")
        self._publishing = False

        if self._publish_thread:
            self._publish_thread.join()

    # ---------------- MQTT API ---------------- #

    def publish(self, topic: str, payload, qos: int = 0, retain: bool = False):
        if isinstance(payload, (dict, list)):
            payload = json.dumps(payload)

        self.client.publish(topic, payload, qos=qos, retain=retain)

    def connect(self):
        self.client.connect(self.host, self.port, self.keepalive)
        self.client.loop_start()

    def disconnect(self):
        self.stop_periodic_publish()
        self.client.loop_stop()
        self.client.disconnect()