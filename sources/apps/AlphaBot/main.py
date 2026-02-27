from Mqtt.mqtt_consumer import MqttConsumer
from robot_controller import RobotController
import time
import paho.mqtt.client as mqtt
import Mqtt.mqtt_configuration as mqtt_configuration

def on_connect(client, userdata, flags, rc):
    print("Connected to mqtt broker")

if __name__ == "__main__":
    client = mqtt.Client()
    client.on_connect = on_connect
    client.connect(mqtt_configuration.MqttBrokerUrl, mqtt_configuration.MqttBrokerPort, mqtt_configuration.MqttKeepalive)
    client.loop_start()

    mqtt_consumer_command = MqttConsumer(client)
    mqtt_consumer_stop_robot = MqttConsumer(client)
    mqtt_consumer_command.subscribe("robot/command")
    mqtt_consumer_stop_robot.subscribe("robot/stop")

    robot_controller = RobotController(client)

    mqtt_consumer_command.add_received_message_handler(robot_controller.start_robot)
    mqtt_consumer_stop_robot.add_received_message_handler(robot_controller.stop_robot)

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Zamykanie...")
    finally:
        mqtt_consumer_command.remove_received_message_handler(robot_controller.start_robot)
        mqtt_consumer_stop_robot.remove_received_message_handler(robot_controller.stop_robot)
        client.loop_stop()
        client.disconnect()