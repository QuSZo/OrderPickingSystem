from .AlphaBot2 import AlphaBot2
from .TRSensors import TRSensor
import time
import json
try:
    import RPi.GPIO as GPIO
except ModuleNotFoundError:
    GPIO = None

BUZ = 4

def beep_on():
	GPIO.output(BUZ,GPIO.HIGH)

def beep_off():
	GPIO.output(BUZ,GPIO.LOW)

def countdown():
	for x in range(1, -1, -1):
		print(x)
		beep_on()
		time.sleep(0.5)
		beep_off()
		time.sleep(0.5)

def publish_status(command, mqtt_client):
	payload = {
		"event": "movement",
		"command": command,
		"timestamp": time.time(),
	}

	mqtt_client.publish("robot/status", json.dumps(payload))

def publish_finished(mqtt_client):
	payload = {
		"event": "finished",
		"timestamp": time.time(),
	}

	mqtt_client.publish("robot/status", json.dumps(payload))

def run_robot(commands, mqtt_client):
	instrukcje = list(commands)
	
	maximum = 60
	integral = 0
	last_proportional = 0

	GPIO.setmode(GPIO.BCM)
	GPIO.setwarnings(False)
	GPIO.setup(BUZ,GPIO.OUT)
	TrSensor = TRSensor()
	AlphaBot = AlphaBot2()
	AlphaBot.stop()

	print("Line follow starting")
	time.sleep(0.5)

	# czy ten obrót jest konieczny?
	for i in range(-4,80):
		if(i<20 or i>= 60):
			AlphaBot.right()	
			AlphaBot.setPWMA(40)
			AlphaBot.setPWMB(40)
		else:
			AlphaBot.left()
			AlphaBot.setPWMA(40)
			AlphaBot.setPWMB(40)
		TrSensor.calibrate()

	AlphaBot.stop()
	print(TrSensor.calibratedMin)
	print(TrSensor.calibratedMax)

	countdown()

	AlphaBot.forward()

	while True:
		try:
			position,Sensors = TrSensor.readLine()
			if(Sensors[0] > 600 and Sensors[1] > 600 and Sensors[2] > 600 and Sensors[3] > 600 and Sensors[4] > 600):
				print("Skrzyżowanie - wybieram kierunek!")
				if (len(instrukcje) == 0):
					publish_finished(mqtt_client)
					AlphaBot.stop()
					break
				else:
					direction = instrukcje.pop(0)
					publish_status(direction, mqtt_client)
					if direction == "left":
						print("Turn: " + direction)
						AlphaBot.left()
						time.sleep(0.5)
					elif direction == "right":
						print("Turn: " + direction)
						AlphaBot.right()
						time.sleep(0.5)
					elif direction == "forward":
						print("Turn: " + direction)
						AlphaBot.setPWMA(maximum)
						AlphaBot.setPWMB(maximum)
						AlphaBot.forward()
						time.sleep(0.2)
					else:
						break
					AlphaBot.forward()
					time.sleep(0.2)
			else:
				proportional = position - 2000
				
				derivative = proportional - last_proportional
				integral += proportional
				
				last_proportional = proportional

				power_difference = proportional/30  + integral/10000 + derivative*2;  

				if (power_difference > maximum):
					power_difference = maximum
				if (power_difference < - maximum):
					power_difference = - maximum
				if (power_difference < 0):
					AlphaBot.setPWMA(maximum + power_difference)
					AlphaBot.setPWMB(maximum)
				else:
					AlphaBot.setPWMA(maximum)
					AlphaBot.setPWMB(maximum - power_difference)
		except KeyboardInterrupt:
			break
