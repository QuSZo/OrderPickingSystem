import threading
import time
import json
import utils


class RobotController:
    def __init__(self, mqtt_client):
        self.client = mqtt_client
        self.commands_list = []
        self._is_running = False
        self._worker_thread = None
        self._lock = threading.Lock()

    def start_robot(self, message):
        data = utils.try_deserialize(message)

        if not data or "commands" not in data:
            print("Niepoprawne dane wejściowe")
            return

        with self._lock:
            self.commands_list.extend(data["commands"])

        if not self._is_running:
            self._is_running = True
            self._worker_thread = threading.Thread(
                target=self._process_commands,
                daemon=True
            )
            self._worker_thread.start()

    def stop_robot(self, *_):
        print("Zatrzymywanie robota...")
        self._is_running = False
        self._publish_stopped()

    def _process_commands(self):
        while self._is_running:
            command = None

            with self._lock:
                if self.commands_list:
                    command = self.commands_list.pop(0)

            if command:
                self._simulate_movement(command)
                time.sleep(3)
            else:
                self._is_running = False
                self._publish_finished()
                break

    def _simulate_movement(self, command):
        print(f"Symulacja ruchu: {command}")

        payload = {
            "event": "movement",
            "command": command,
            "timestamp": time.time(),
        }

        self.client.publish("robot/status", json.dumps(payload))

    def _publish_finished(self):
        print("Wszystkie komendy wykonane")

        payload = {
            "event": "finished",
            "timestamp": time.time(),
        }

        self.client.publish("robot/status", json.dumps(payload))

    def _publish_stopped(self):
        payload = {
            "event": "stopped",
            "timestamp": time.time(),
        }

        self.client.publish("robot/status", json.dumps(payload))