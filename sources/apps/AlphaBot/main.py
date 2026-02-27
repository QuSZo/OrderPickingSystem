from mqtt_subscriber import ControlledPublisher
import time

if __name__ == "__main__":
    app = ControlledPublisher(
        host="mqtt-broker",
        port=1883,
        client_id="robot_controlled"
    )

    app.connect()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Zamykanie...")
        app.disconnect()