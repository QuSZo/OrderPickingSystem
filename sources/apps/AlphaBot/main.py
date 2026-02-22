from mqtt_publisher import MqttPublisher
import time

if __name__ == "__main__":
    publisher = MqttPublisher(
        host="localhost",
        port=1883,
        client_id="robot_publisher"
    )

    publisher.connect()
    publisher.start_periodic_publish("robot/status", interval=1.0)

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Zatrzymywanie...")
        publisher.disconnect()