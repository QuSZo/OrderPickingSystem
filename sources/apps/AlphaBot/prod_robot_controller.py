from RobotDriver import Line_Follow
import utils
import threading

class ProdRobotController:
    def __init__(self, mqtt_client):
        self.client = mqtt_client
    
    def start_robot(self, message):
        print("Start robota")
        data = utils.try_deserialize(message)

        if not data or "commands" not in data:
            print("Niepoprawne dane wejściowe")
            return

        threading.Thread(
            target=Line_Follow.run_robot,
            args=(data["commands"], self.client),
            daemon=True
        ).start()

    def stop_robot(self, *_):
        print("Zatrzymanie robota")