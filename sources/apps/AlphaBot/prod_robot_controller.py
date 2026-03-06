class ProdRobotController:
    def __init__(self, mqtt_client):
        self.client = mqtt_client
        self.commands_list = []
    
    def start_robot(self, message):
        print("Start robota")

    def stop_robot(self, *_):
        print("Zatrzymanie robota")